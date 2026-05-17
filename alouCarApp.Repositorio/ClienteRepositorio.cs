using AlouCar.Dominio.Entidades;
using AlouCar.Repositorio.Contexto;
using AlouCar.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlouCar.Repositorio
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly AlouCarContexto _contexto;

        public ClienteRepositorio(AlouCarContexto contexto)
        {
            _contexto = contexto;
        }

        public int Criar(Cliente cliente)
        {
            _contexto.Clientes.Add(cliente);
            _contexto.SaveChanges();
            return cliente.Id;
        }

        public void Atualizar(Cliente cliente)
        {
            _contexto.Clientes.Update(cliente);
            _contexto.SaveChanges();
        }

        public Cliente Obter(int id)
        {
            return _contexto.Clientes.FirstOrDefault(c => c.Id == id);
        }

        public async Task<List<Cliente>> Listar(bool ativo)
        {
            return await _contexto.Clientes
                .Where(c => c.Ativo == ativo)
                .ToListAsync();
        }

        public async Task<bool> Excluir(Cliente cliente)
        {
            cliente.Deletar();
            _contexto.Clientes.Update(cliente);
            await _contexto.SaveChangesAsync();
            return true;
        }
    }
}