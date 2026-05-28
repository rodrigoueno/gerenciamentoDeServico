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
        private readonly DbConnectionDapper _dapper;

        public ServicoRepositorio(AlouCarContexto contexto, DbConnectionDapper factory)
        {
            _contexto = contexto;
            _dapper = factory;
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
            using var caminhoDapper = _dapper.Create();
            var pesquisaServico = new Dictionary<int, Servico>();

            await caminhoDapper.QueryAsync<Servico, Cliente, Veiculo, ServicoItem, Servico>(
                "sp_Servico_Read",
                (servico, cliente, veiculo, item) =>
                {
                    if (!pesquisaServico.TryGetValue(servico.Id, out var servicoObter))
                    {
                        servicoObter = servico;
                        servicoObter.Cliente = cliente;
                        servicoObter.Veiculo = veiculo;
                        servicoObter.Itens = new List<ServicoItem>();
                        pesquisaServico[servicoObter.Id] = servicoObter;
                    }
                    if (item != null) servicoObter.Itens.Add(item);
                    return servicoObter;
                },
                new { Id = id },
                splitOn: "ClienteId,VeiculoId,ItemId",
                commandType: CommandType.StoredProcedure);

            return pesquisaServico.Values.FirstOrDefault();
        }

        public async Task<List<Servico>> Listar(bool ativo)
        {
            using var caminhoDapper = _dapper.Create();
            var pesquisaServico = new Dictionary<int, Servico>();

            await caminhoDapper.QueryAsync<Servico, Cliente, Veiculo, ServicoItem, Servico>(
                "sp_Servico_Read",
                (servico, cliente, veiculo, item) =>
                {
                    if (!pesquisaServico.TryGetValue(servico.Id, out var servicoListar))
                    {
                        servicoListar = servico;
                        servicoListar.Cliente = cliente;
                        servicoListar.Veiculo = veiculo;
                        servicoListar.Itens = new List<ServicoItem>();
                        pesquisaServico[servicoListar.Id] = servicoListar;
                    }
                    if (item != null) servicoListar.Itens.Add(item);
                    return servicoListar;
                },
                new { Ativo = ativo },
                splitOn: "ClienteId,VeiculoId,ItemId",
                commandType: CommandType.StoredProcedure);

            return pesquisaServico.Values.ToList();
        }

        public async Task<List<Servico>> ListarPorCliente(int clienteId)
        {
            using var caminhoDapper = _dapper.Create();
            var pesquisaCliente = new Dictionary<int, Servico>();

            await caminhoDapper.QueryAsync<Servico, Veiculo, ServicoItem, Servico>(
                "sp_Servico_ReadByCliente",
                (servico, veiculo, item) =>
                {
                    if (!pesquisaCliente.TryGetValue(servico.Id, out var servicoListar))
                    {
                        servicoListar = servico;
                        servicoListar.Veiculo = veiculo;
                        servicoListar.Itens = new List<ServicoItem>();
                        pesquisaCliente[servicoListar.Id] = servicoListar;
                    }
                    if (item != null) servicoListar.Itens.Add(item);
                    return servicoListar;
                },
                new { ClienteId = clienteId },
                splitOn: "VeiculoId,ItemId",
                commandType: CommandType.StoredProcedure);

            return pesquisaCliente.Values.ToList();
        }
    }
}