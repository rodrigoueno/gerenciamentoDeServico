using AlouCar.Dominio.Enumeradores;

namespace alouCarApp.API.Models
{
    public class ServicoAtualizar
    {
        public int Id { get; set; }
        public TipoServico TipoServico { get; set; }
        public DateTime DataAgendamento { get; set; }
        public decimal ValorPrevisto { get; set; }
        public decimal ValorTotal { get; set; }
        public string Observacao { get; set; }
        public SituacaoServico Situacao { get; set; }
        public DateTime DataConclusao { get; set; }
    }
}