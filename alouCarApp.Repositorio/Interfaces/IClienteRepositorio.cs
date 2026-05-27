using AlouCar.Dominio.Entidades;

namespace AlouCar.Repositorio.Interfaces
{
    public interface IClienteRepositorio
    {
        int Criar(Cliente cliente);

        Task Atualizar(Cliente cliente);

        Task<Cliente> Obter(int id);

        Task<List<Cliente>> Listar(bool ativo);

        Task<bool> Excluir(Cliente cliente);
    }
}