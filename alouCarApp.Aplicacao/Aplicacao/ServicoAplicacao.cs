using AlouCar.Aplicacao.Interfaces;
using AlouCar.Dominio.Entidades;
using AlouCar.Repositorio.Interfaces;
using AlouCar.Dominio.Enumeradores;

namespace AlouCar.Aplicacao.Aplicacoes
{
    public class ServicoAplicacao : IServicoAplicacao
    {
        readonly IServicoRepositorio _servicoRepositorio;
        readonly IClienteRepositorio _clienteRepositorio;
        readonly IVeiculoRepositorio _veiculoRepositorio;

        public ServicoAplicacao(IServicoRepositorio servicoRepositorio, IClienteRepositorio clienteRepositorio, IVeiculoRepositorio veiculoRepositorio)
        {
            _servicoRepositorio = servicoRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _veiculoRepositorio = veiculoRepositorio;
        }

        public int Criar(Servico servico)
        {
            if (servico == null)
                throw new Exception("Serviço não pode ser vazio");

            ValidarInformacaoServico(servico);
            ValidarCliente(servico.ClienteId);
            ValidarVeiculo(servico.VeiculoId, servico.ClienteId).Wait();
            return _servicoRepositorio.Criar(servico);
        }

        public void Atualizar(Servico servico)
        {
            var servicoDominio = _servicoRepositorio.ObterPorId(servico.Id).Result;
            if (servicoDominio == null)
                throw new Exception("Serviço não encontrado");

            ValidarInformacaoServico(servico);
            servicoDominio.TipoServico = servico.TipoServico;
            servicoDominio.DataAgendamento = servico.DataAgendamento;
            servicoDominio.Situacao = servico.Situacao;
            servicoDominio.ValorPrevisto = servico.ValorPrevisto;
            servicoDominio.ValorTotal = servico.ValorTotal;
            servicoDominio.Observacao = servico.Observacao;

            if (servico.Situacao == SituacaoServico.Concluido)
                servicoDominio.Concluir();

            _servicoRepositorio.Atualizar(servicoDominio);
        }

        public async Task<Servico> ObterPorId(int id)
        {
            var servicoDominio = await _servicoRepositorio.ObterPorId(id);
            if (servicoDominio == null)
                throw new Exception("Serviço não encontrado");
            return servicoDominio;
        }

        public async Task<List<Servico>> Listar(bool ativo)
        {
            return await _servicoRepositorio.Listar(ativo);
        }

        public async Task<List<Servico>> ListarPorCliente(int clienteId)
        {
            return await _servicoRepositorio.ListarPorCliente(clienteId);
        }

        public async Task<bool> Excluir(Servico servico)
        {
            var servicoDominio = await _servicoRepositorio.ObterPorId(servico.Id);
            if (servicoDominio == null)
                throw new Exception("Serviço não encontrado");

            return await _servicoRepositorio.Excluir(servicoDominio);
        }

        #region Util
        private static void ValidarInformacaoServico(Servico servico)
        {
            if (servico.DataAgendamento == default)
                throw new Exception("Data de agendamento não pode ser vazia");
            if (servico.ValorPrevisto <= 0)
                throw new Exception("Valor previsto deve ser maior que zero");
        }

        private void ValidarCliente(int clienteId)
        {
            var cliente = _clienteRepositorio.Obter(clienteId);
            if (cliente == null)
                throw new Exception($"Cliente {clienteId} não encontrado");
            if (!cliente.Ativo)
                throw new Exception($"Cliente {clienteId} está inativo");
        }

        private async Task ValidarVeiculo(int veiculoId, int clienteId)
        {
            var veiculo = await _veiculoRepositorio.ObterPorId(veiculoId);
            if (veiculo == null)
                throw new Exception($"Veículo {veiculoId} não encontrado");
            if (!veiculo.Ativo)
                throw new Exception($"Veículo {veiculoId} está inativo");
            if (veiculo.ClienteId != clienteId)
                throw new Exception($"Veículo {veiculoId} não pertence ao cliente {clienteId}");
        }
        #endregion
    }
}