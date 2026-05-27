using AlouCar.Dominio.Enumeradores;

namespace alouCarApp.API.Models.Servicos
{
    public class ServicoAtualizar
    {
        public DateTime DataAgendamento { get; set; }
        public decimal ValorTotal { get; set; }
        public string Observacao { get; set; }
        public SituacaoServico Situacao { get; set; }
    }
}