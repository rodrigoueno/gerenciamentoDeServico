namespace alouCarApp.API.Models.Servicos
{
    public class ServicoCriar
    {
        public int ClienteId { get; set; }
        public int VeiculoId { get; set; }
        public DateTime DataAgendamento { get; set; }
        public string Observacao { get; set; }

        public List<ServicoItemCriar> Itens { get; set; } = new();
    }
}