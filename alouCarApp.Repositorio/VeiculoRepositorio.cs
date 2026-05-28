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
        private readonly DbConnectionDapper _dapper;

        public VeiculoRepositorio(AlouCarContexto contexto, DbConnectionDapper dapper)
        {
            _contexto = contexto;
            _dapper  = dapper;
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

        public async Task<Veiculo> ObterPorId(int id)
        {
            using var caminhoDapper = _dapper.Create();
            return await caminhoDapper.QueryFirstOrDefaultAsync<Veiculo>(
                "sp_Veiculo_Read",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<Veiculo>> Listar(bool ativo)
        {
            using var caminhoDapper = _dapper.Create();
            var resultado = await caminhoDapper.QueryAsync<Veiculo>(
                "sp_Veiculo_Read",
                new { Ativo = ativo },
                commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }

        public async Task<List<Veiculo>> ListarPorCliente(int clienteId)
        {
            using var caminhoDapper = _dapper.Create();
            var resultado = await caminhoDapper.QueryAsync<Veiculo>(
                "sp_Veiculo_ReadByCliente",
                new { ClienteId = clienteId },
                commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }
    }
}