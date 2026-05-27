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
                    new { role = "user", content = prompt }
                },
                tools = new[]
                {
                    new
                    {
                        type = "function",
                        function = new
                        {
                            name = "web_search",
                            description = "Pesquisa informações públicas na internet sobre vida útil e intervalos de manutenção de itens automotivos para uma marca e modelo específicos.",
                            parameters = new
                            {
                                type = "object",
                                properties = new
                                {
                                    query = new
                                    {
                                        type = "string",
                                        description = "Consulta de pesquisa, ex: 'intervalo troca óleo Fiat Uno 2015 manual fabricante km'"
                                    }
                                },
                                required = new[] { "query" }
                            }
                        }
                    }
                },
                tool_choice = "auto"
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

            // Percorre choices até encontrar conteúdo text final
            foreach (var choice in choices.EnumerateArray())
            {
                var message = choice.GetProperty("message");

                // Se tem content direto (resposta final)
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

            sb.AppendLine("Você é um especialista em manutenção automotiva.");
            sb.AppendLine("Siga obrigatoriamente as etapas abaixo antes de responder:");
            sb.AppendLine();
            sb.AppendLine("ETAPA 1 — PESQUISA:");
            sb.AppendLine($"Pesquise na internet os intervalos de manutenção recomendados pelo fabricante e por boas práticas do mercado para:");
            sb.AppendLine($"  Veículo: {veiculo.Marca} {veiculo.Modelo} {veiculo.AnoFabricacao}/{veiculo.AnoModelo}");
            sb.AppendLine("  Itens a pesquisar: troca de óleo, filtro de óleo, filtro de ar, filtro de combustível,");
            sb.AppendLine("  velas, pastilhas de freio, fluido de freio, correia dentada, pneus, bateria e revisão geral.");
            sb.AppendLine("  Use os resultados da pesquisa como base de vida útil para a análise.");
            sb.AppendLine();
            sb.AppendLine("ETAPA 2 — ANÁLISE:");
            sb.AppendLine($"KM atual do veículo: {kilometragemAtual} km");
            sb.AppendLine();
            sb.AppendLine("Histórico de serviços realizados (com KM no momento de cada revisão):");

            if (historico == null || !historico.Any())
            {
                sb.AppendLine("  - Nenhum serviço registrado.");
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
            sb.AppendLine("Para cada item do histórico que tem KM registrado, calcule:");
            sb.AppendLine("  KM rodados desde a última troca = KM atual - KM na revisão");
            sb.AppendLine("  Compare com o intervalo encontrado na pesquisa.");
            sb.AppendLine("  >= 100% do intervalo → prioridade Alta");
            sb.AppendLine("  >= 80% do intervalo  → prioridade Média");
            sb.AppendLine("  < 80% do intervalo   → prioridade Baixa");
            sb.AppendLine("  Item nunca realizado  → prioridade Alta");
            sb.AppendLine();
            sb.AppendLine("ETAPA 3 — RESPOSTA:");
            sb.AppendLine("Retorne SOMENTE o JSON abaixo, sem texto adicional, sem markdown:");
            sb.AppendLine(@"{
  ""servicosSugeridos"": [
    {
      ""tipoServico"": ""Nome do serviço"",
      ""justificativa"": ""Ex: Último troca em 42.000 km, intervalo recomendado 5.000 km, KM atual 48.000 km — vencido há 1.000 km"",
      ""prioridade"": ""Alta""
    }
  ],
  ""previsaoProximoRetorno"": ""Ex: Retornar em 5.000 km ou 6 meses, o que ocorrer primeiro""
}");

            return sb.ToString();
        }
    }
}