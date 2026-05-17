using AlouCar.Dominio.Enumeradores;

namespace alouCarApp.API.Models.Servicos.Resposta
{
    public class ServicoResponse
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string NomeCliente { get; set; }
        public int VeiculoId { get; set; }
        public string ModeloVeiculo { get; set; }
        public string PlacaVeiculo { get; set; }
        public TipoServico TipoServico { get; set; }
        public DateTime DataAgendamento { get; set; }
        public SituacaoServico Situacao { get; set; }
        public decimal ValorPrevisto { get; set; }
        public decimal ValorTotal { get; set; }
        public string Observacao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}