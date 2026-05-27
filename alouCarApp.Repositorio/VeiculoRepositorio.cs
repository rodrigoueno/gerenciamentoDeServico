using AlouCar.Dominio.Entidades;
using AlouCar.Repositorio.Contexto;
using AlouCar.Repositorio.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AlouCar.Repositorio
{
    public class VeiculoRepositorio : IVeiculoRepositorio
    {
        private readonly AlouCarContexto _contexto;
        private readonly DbConnectionFactory _factory;

        public VeiculoRepositorio(AlouCarContexto contexto, DbConnectionFactory factory)
        {
            _contexto = contexto;
            _factory  = factory;
        }

        // ── ESCRITA (Entity Framework) ────────────────────────────────────────

        public int Criar(Veiculo veiculo)
        {
            _contexto.Veiculos.Add(veiculo);
            _contexto.SaveChanges();
            return veiculo.Id;
        }

        public void Atualizar(Veiculo veiculo)
        {
            _contexto.Veiculos.Update(veiculo);
            _contexto.SaveChanges();
        }

        public async Task<bool> Excluir(Veiculo veiculo)
        {
            veiculo.Deletar();
            _contexto.Veiculos.Update(veiculo);
            await _contexto.SaveChangesAsync();
            return true;
        }

        // ── LEITURA (Dapper + Stored Procedures) ─────────────────────────────

        public async Task<Veiculo> ObterPorId(int id)
        {
            using var db = _factory.Create();
            return await db.QueryFirstOrDefaultAsync<Veiculo>(
                "sp_Veiculo_Read",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<Veiculo>> Listar(bool ativo)
        {
            using var db = _factory.Create();
            var resultado = await db.QueryAsync<Veiculo>(
                "sp_Veiculo_Read",
                new { Ativo = ativo },
                commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }

        public async Task<List<Veiculo>> ListarPorCliente(int clienteId)
        {
            using var db = _factory.Create();
            var resultado = await db.QueryAsync<Veiculo>(
                "sp_Veiculo_ReadByCliente",
                new { ClienteId = clienteId },
                commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }
    }
}