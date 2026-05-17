using AlouCar.Dominio.Entidades;

namespace AlouCar.Repositorio.Interfaces
{
    public interface IClienteRepositorio
    {
        int Criar(Cliente cliente);
        void Atualizar(Cliente cliente);
        Cliente Obter(int id);
        Task<List<Cliente>> Listar(bool ativo);
        Task<bool> Excluir(Cliente cliente);
    }
}