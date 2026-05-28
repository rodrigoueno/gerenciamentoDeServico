using AlouCar.Dominio.Entidades;
using AlouCar.Repositorio.Contexto;
using AlouCar.Repositorio.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AlouCar.Repositorio
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly AlouCarContexto _contexto;
        private readonly DbConnectionDapper _dapper;

        public ClienteRepositorio(AlouCarContexto contexto, DbConnectionDapper factory)
        {
            _contexto = contexto;
            _dapper = factory;
        }


        public int Criar(Cliente cliente)
        {
            _contexto.Clientes.Add(cliente);
            _contexto.SaveChanges();
            return cliente.Id;
        }

        public async Task Atualizar(Cliente cliente)
        {
            _contexto.Clientes.Update(cliente);
            await _contexto.SaveChangesAsync();
        }

        public async Task<bool> Excluir(Cliente cliente)
        {
            cliente.Deletar();
            _contexto.Clientes.Update(cliente);
            await _contexto.SaveChangesAsync();
            return true;
        }

        public async Task<Cliente> Obter(int id)
        {
            using var caminhoDapper = _dapper.Create();
            return await caminhoDapper.QueryFirstOrDefaultAsync<Cliente>(
                "sp_Cliente_Read",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<Cliente>> Listar(bool ativo)
        {
            using var CaminhoDapper = _dapper.Create();
            var resultado = await CaminhoDapper.QueryAsync<Cliente>(
                "sp_Cliente_Read",
                new { Ativo = ativo },
                commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }
    }
}