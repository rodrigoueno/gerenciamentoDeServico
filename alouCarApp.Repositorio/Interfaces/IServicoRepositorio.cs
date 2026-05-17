using AlouCar.Dominio.Entidades;

namespace AlouCar.Repositorio.Interfaces
{
    public interface IServicoRepositorio
    {
        int Criar(Servico servico);
        void Atualizar(Servico servico);
        Task<Servico> ObterPorId(int id);
        Task<List<Servico>> Listar(bool ativo);
        Task<List<Servico>> ListarPorCliente(int clienteId);
        Task<bool> Excluir(Servico servico);
    }
}