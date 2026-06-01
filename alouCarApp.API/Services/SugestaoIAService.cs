using AlouCar.Dominio.Entidades;
using alouCarApp.API.Models.IA;
using System.Text;
using System.Text.Json;

namespace alouCarApp.API.Services
{
    public class SugestaoIAService : ISugestaoIAService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public SugestaoIAService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Groq:ApiKey"];
        }

        public async Task<SugestaoIAResponse> ObterSugestoes(
            Veiculo veiculo,
            int kilometragemAtual,
            List<Servico> historico)
        {
            var prompt = MontarPrompt(veiculo, kilometragemAtual, historico);

            var requestBody = new
            {
                model = "meta-llama/llama-4-scout-17b-16e-instruct",
                max_tokens = 2000,
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = "Você é um especialista em manutenção automotiva com amplo conhecimento dos intervalos recomendados pelos fabricantes brasileiros. Responda sempre em português do Brasil. Retorne SOMENTE JSON válido, sem markdown, sem texto adicional."
                    },
                    new { role = "user", content = prompt }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.groq.com/openai/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var texto = ExtrairTextoResposta(responseBody);

            var textoLimpo = texto
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            return JsonSerializer.Deserialize<SugestaoIAResponse>(textoLimpo, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private static string ExtrairTextoResposta(string responseBody)
        {
            var jsonDoc = JsonDocument.Parse(responseBody);
            var choices = jsonDoc.RootElement.GetProperty("choices");

            foreach (var choice in choices.EnumerateArray())
            {
                var message = choice.GetProperty("message");

                if (message.TryGetProperty("content", out var content)
                    && content.ValueKind == JsonValueKind.String)
                {
                    var texto = content.GetString();
                    if (!string.IsNullOrWhiteSpace(texto))
                        return texto;
                }
            }

            throw new InvalidOperationException("Não foi possível extrair resposta de texto da IA.");
        }

        private static string MontarPrompt(
            Veiculo veiculo,
            int kilometragemAtual,
            List<Servico> historico)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Analise o veículo abaixo e gere sugestões de manutenção com base no seu conhecimento dos intervalos recomendados pelo fabricante:");
            sb.AppendLine();
            sb.AppendLine($"Veículo: {veiculo.Marca} {veiculo.Modelo} {veiculo.AnoFabricacao}/{veiculo.AnoModelo}");
            sb.AppendLine($"KM atual: {kilometragemAtual} km");
            sb.AppendLine();
            sb.AppendLine("Histórico de serviços realizados:");

            if (historico == null || !historico.Any())
            {
                sb.AppendLine("  Nenhum serviço registrado.");
            }
            else
            {
                foreach (var servico in historico.OrderByDescending(s => s.DataCriacao))
                {
                    sb.AppendLine($"  Serviço em {servico.DataCriacao:dd/MM/yyyy} | Status: {servico.Situacao}");
                    foreach (var item in servico.Itens)
                    {
                        var kmInfo = item.KilometragemNaRevisao.HasValue
                            ? $"KM na revisão: {item.KilometragemNaRevisao.Value} km"
                            : "KM não registrado";
                        sb.AppendLine($"    • {item.TipoServico} ({kmInfo})");
                    }
                }
            }

            sb.AppendLine();
            sb.AppendLine("Regras de prioridade:");
            sb.AppendLine("  >= 100% do intervalo recomendado → Alta");
            sb.AppendLine("  >= 80% do intervalo recomendado  → Média");
            sb.AppendLine("  < 80% do intervalo recomendado   → Baixa");
            sb.AppendLine("  Item nunca realizado              → Alta");
            sb.AppendLine();
            sb.AppendLine("Retorne SOMENTE o JSON abaixo, sem nenhum texto adicional:");
            sb.AppendLine(@"{
  ""servicosSugeridos"": [
    {
      ""tipoServico"": ""Nome do serviço"",
      ""justificativa"": ""Explicação baseada no KM e histórico"",
      ""prioridade"": ""Alta""
    }
  ],
  ""previsaoProximoRetorno"": ""Ex: Retornar em 5.000 km ou 6 meses""
}");

            return sb.ToString();
        }
    }
}