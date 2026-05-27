using AlouCar.Dominio.Entidades;

namespace AlouCar.Repositorio.Interfaces
{
    public interface IVeiculoRepositorio
    {
        int Criar(Veiculo veiculo);

        void Atualizar(Veiculo veiculo);

        Task<Veiculo> ObterPorId(int id);

        Task<List<Veiculo>> Listar(bool ativo);

        Task<List<Veiculo>> ListarPorCliente(int clienteId);

        Task<bool> Excluir(Veiculo veiculo);
    }
}