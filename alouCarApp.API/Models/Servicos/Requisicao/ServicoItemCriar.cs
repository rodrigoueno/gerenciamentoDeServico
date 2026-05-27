using AlouCar.Dominio.Enumeradores;

namespace alouCarApp.API.Models.Servicos
{
    public class ServicoItemCriar
    {
        public TipoServico TipoServico { get; set; }
        public decimal Valor { get; set; }
    }
}