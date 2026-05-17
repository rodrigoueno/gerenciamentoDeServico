using AlouCar.Dominio.Entidades;

namespace AlouCar.Aplicacao.Interfaces
{
    public interface IServicoAplicacao
    {
        int Criar(Servico servico);
        void Atualizar(Servico servico);
        Task<List<Servico>> Listar(bool ativo);
        Task<List<Servico>> ListarPorCliente(int clienteId);
        Task<Servico> ObterPorId(int id);
        Task<bool> Excluir(Servico servico);
    }
}