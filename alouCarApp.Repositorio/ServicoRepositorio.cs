using AlouCar.Dominio.Entidades;
using AlouCar.Repositorio.Contexto;
using AlouCar.Repositorio.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AlouCar.Repositorio
{
    public class ServicoRepositorio : IServicoRepositorio
    {
        private readonly AlouCarContexto _contexto;
        private readonly DbConnectionFactory _factory;

        public ServicoRepositorio(AlouCarContexto contexto, DbConnectionFactory factory)
        {
            _contexto = contexto;
            _factory = factory;
        }



        public int Criar(Servico servico)
        {
            _contexto.Servicos.Add(servico);
            _contexto.SaveChanges();
            return servico.Id;
        }

        public void Atualizar(Servico servico)
        {
            _contexto.Entry(servico).State = EntityState.Modified;
            if (servico.Cliente != null)
                _contexto.Entry(servico.Cliente).State = EntityState.Detached;
            if (servico.Itens != null)
                foreach (var item in servico.Itens)
                    _contexto.Entry(item).State = EntityState.Detached;



            //_contexto.Servicos.Update(servico);
            _contexto.SaveChanges();
        }

        public async Task<bool> Excluir(Servico servico)
        {
            var servicoExcluir = await _contexto.Servicos
                .FirstOrDefaultAsync(s => s.Id == servico.Id);

            if (servicoExcluir == null) return false;

            servicoExcluir.Deletar();
            await _contexto.SaveChangesAsync();
            return true;
        }


        public async Task<Servico> ObterPorId(int id)
        {
            using var db = _factory.Create();
            var lookup = new Dictionary<int, Servico>();

            await db.QueryAsync<Servico, Cliente, Veiculo, ServicoItem, Servico>(
                "sp_Servico_Read",
                (servico, cliente, veiculo, item) =>
                {
                    if (!lookup.TryGetValue(servico.Id, out var s))
                    {
                        s = servico;
                        s.Cliente = cliente;
                        s.Veiculo = veiculo;
                        s.Itens = new List<ServicoItem>();
                        lookup[s.Id] = s;
                    }
                    if (item != null) s.Itens.Add(item);
                    return s;
                },
                new { Id = id },
                splitOn: "ClienteId,VeiculoId,ItemId",
                commandType: CommandType.StoredProcedure);

            return lookup.Values.FirstOrDefault();
        }

        public async Task<List<Servico>> Listar(bool ativo)
        {
            using var db = _factory.Create();
            var lookup = new Dictionary<int, Servico>();

            await db.QueryAsync<Servico, Cliente, Veiculo, ServicoItem, Servico>(
                "sp_Servico_Read",
                (servico, cliente, veiculo, item) =>
                {
                    if (!lookup.TryGetValue(servico.Id, out var s))
                    {
                        s = servico;
                        s.Cliente = cliente;
                        s.Veiculo = veiculo;
                        s.Itens = new List<ServicoItem>();
                        lookup[s.Id] = s;
                    }
                    if (item != null) s.Itens.Add(item);
                    return s;
                },
                new { Ativo = ativo },
                splitOn: "ClienteId,VeiculoId,ItemId",
                commandType: CommandType.StoredProcedure);

            return lookup.Values.ToList();
        }

        public async Task<List<Servico>> ListarPorCliente(int clienteId)
        {
            using var db = _factory.Create();
            var lookup = new Dictionary<int, Servico>();

            await db.QueryAsync<Servico, Veiculo, ServicoItem, Servico>(
                "sp_Servico_ReadByCliente",
                (servico, veiculo, item) =>
                {
                    if (!lookup.TryGetValue(servico.Id, out var s))
                    {
                        s = servico;
                        s.Veiculo = veiculo;
                        s.Itens = new List<ServicoItem>();
                        lookup[s.Id] = s;
                    }
                    if (item != null) s.Itens.Add(item);
                    return s;
                },
                new { ClienteId = clienteId },
                splitOn: "VeiculoId,ItemId",
                commandType: CommandType.StoredProcedure);

            return lookup.Values.ToList();
        }
    }
}