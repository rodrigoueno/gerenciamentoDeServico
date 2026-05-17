using AlouCar.Dominio.Entidades;

namespace AlouCar.Aplicacao.Interfaces
{
    public interface IVeiculoAplicacao
    {
        int Criar(Veiculo veiculo);
        void Atualizar(Veiculo veiculo);
        Task<List<Veiculo>> Listar(bool ativo);
        Task<List<Veiculo>> ListarPorCliente(int clienteId);
        Task<Veiculo> ObterPorId(int id);
        Task<bool> Excluir(Veiculo veiculo);
    }
}