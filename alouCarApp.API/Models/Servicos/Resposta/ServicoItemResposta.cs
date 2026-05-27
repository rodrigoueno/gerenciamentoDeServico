using AlouCar.Dominio.Enumeradores;

namespace alouCarApp.API.Models.Servicos.Resposta
{
    public class ServicoItemResposta
    {
        public int Id { get; set; }
        public TipoServico TipoServico { get; set; }
        public decimal Valor { get; set; }
    }
}