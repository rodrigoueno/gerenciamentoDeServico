using AlouCar.Dominio.Enumeradores;

namespace AlouCar.Dominio.Entidades
{
    public class Servico
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int VeiculoId { get; set; }
        public TipoServico TipoServico { get; set; }
        public DateTime DataAgendamento { get; set; }
        public SituacaoServico Status { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataConclusao { get; set; }
        public Cliente Cliente { get; set; }
        public Veiculo Veiculo { get; set; }
        public string Observacao { get; set; }
        public bool Ativo { get; set; }

        public Servico()
        {
            Ativo = true;
            DataCriacao = DateTime.Now;
            Status = SituacaoServico.Agendado;
        }

        public void Deletar()
        {
            Status = SituacaoServico.Cancelado;
            Ativo = false;
        }

    }
}