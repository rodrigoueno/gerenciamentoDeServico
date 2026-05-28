namespace alouCarApp.API.Models.IA
{
    public class SugestaoIAResponse
    {
        public List<ServicoSugerido> ServicosSugeridos { get; set; } = new();
        public string PrevisaoProximoRetorno { get; set; }
    }

    public class ServicoSugerido
    {
        public string TipoServico { get; set; }
        public string Justificativa { get; set; }
        public string Prioridade { get; set; }
    }
}