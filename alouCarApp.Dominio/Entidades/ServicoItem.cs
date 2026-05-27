using AlouCar.Dominio.Enumeradores;

namespace AlouCar.Dominio.Entidades
{
    public class ServicoItem
    {
        public int Id { get; set; }
        public int ServicoId { get; set; }
        public int? KilometragemNaRevisao { get; set; }
        public TipoServico TipoServico { get; set; }
        public decimal Valor { get; set; }
        public Servico Servico { get; set; }
    }
}