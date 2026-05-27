using AlouCar.Dominio.Entidades;

namespace AlouCar.Aplicacao.Interfaces
{
    public interface IClienteAplicacao
    {
        int Criar(Cliente cliente);

        Task Atualizar(Cliente cliente);

        Task<List<Cliente>> Listar(bool ativo);

        Task<Cliente> ObterPorId(int id);

        Task<bool> Excluir(Cliente cliente);
    }
}