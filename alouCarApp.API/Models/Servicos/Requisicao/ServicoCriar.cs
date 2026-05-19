using AlouCar.Dominio.Enumeradores;

namespace alouCarApp.API.Models.Servicos
{
    public class ServicoCriar
    {
        public int ClienteId { get; set; }
        public int VeiculoId { get; set; }
        public TipoServico TipoServico { get; set; }
        public DateTime DataAgendamento { get; set; }
        public decimal ValorPrevisto { get; set; }
        public string Observacao { get; set; }
    }
}