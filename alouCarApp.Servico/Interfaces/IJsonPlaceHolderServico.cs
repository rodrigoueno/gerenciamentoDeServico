using AlouCar.Dominio.Entidades;

namespace AlouCar.Servicos.Interfaces
{
    public interface IJsonPlaceHolderServico
    {
        Task<List<Servico>> ListarServicos();
    }
}