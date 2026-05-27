using AlouCar.Dominio.Entidades;
using AlouCar.Dominio.Enumeradores;

namespace AlouCar.Aplicacao.Interfaces
{
    public interface IServicoAplicacao
    {
        Task<int> Criar(Servico servico);
        Task Atualizar(Servico servico);
        Task<List<Servico>> Listar(bool ativo);
        Task<List<Servico>> ListarPorCliente(int clienteId);
        Task<Servico> ObterPorId(int id);
        Task<bool> Excluir(Servico servico);
        void AtualizarSituacao(int id, SituacaoServico situacao);
    }
}