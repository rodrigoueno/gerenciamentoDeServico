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

        public async Task<int> Criar(Servico servico)
        {
            if (servico == null)
                throw new Exception("Serviço não pode ser vazio");

            if (servico.Itens == null || !servico.Itens.Any())
                throw new Exception("O serviço deve ter ao menos um item");

            if (servico.Itens.Any(i => i.Valor <= 0))
                throw new Exception("Todos os itens devem ter valor maior que zero");

            servico.ValorPrevisto = servico.Itens.Sum(i => i.Valor);

            ValidarInformacaoServico(servico);
            await ValidarCliente(servico.ClienteId);
            await ValidarVeiculo(servico.VeiculoId, servico.ClienteId);

            return _servicoRepositorio.Criar(servico);
        }

        public async Task Atualizar(Servico servico)
        {
            var servicoAtualizar = await _servicoRepositorio.ObterPorId(servico.Id);

            if (servicoAtualizar == null)
            {
                throw new Exception("Serviço não encontrado");
            }
            if (servico.DataAgendamento != default)
            {
                servicoAtualizar.DataAgendamento = servico.DataAgendamento;
            }
            if (servico.ValorTotal > 0)
            {
                servicoAtualizar.ValorTotal = servico.ValorTotal;
            }
            if (!string.IsNullOrEmpty(servico.Observacao))
            {
                servicoAtualizar.Observacao = servico.Observacao;
            }
            if (servico.Situacao != default)
            {
                servicoAtualizar.Situacao = servico.Situacao;
            }
            if (servico.Situacao == SituacaoServico.Concluido)
            {
                servicoAtualizar.Concluir();
            }

            _servicoRepositorio.Atualizar(servicoAtualizar);
        }

        public async Task<Servico> ObterPorId(int id)
        {
            var servicoObter = await _servicoRepositorio.ObterPorId(id);
            if (servicoObter == null)
            {
                throw new Exception("Serviço não encontrado");
            }
            return servicoObter;
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
            if (servico == null)
            {
                throw new Exception("Serviço não encontrado");
            }

            return await _servicoRepositorio.Excluir(servico);
        }

        public void AtualizarSituacao(int id, SituacaoServico situacao)
        {
            var servicoSituacao = _servicoRepositorio.ObterPorId(id).Result;

            if (servicoSituacao == null)
                throw new Exception("Serviço não encontrado");
            switch (situacao)
            {
                case SituacaoServico.EmAndamento:
                    servicoSituacao.Iniciar();
                    break;

                case SituacaoServico.Concluido:
                    servicoSituacao.Concluir();
                    break;

                case SituacaoServico.Cancelado:
                    throw new Exception("Para cancelar um serviço utilize o endpoint de exclusão");

                default:
                    throw new Exception($"Situação '{situacao}' inválida para atualização");
            }
            _servicoRepositorio.Atualizar(servicoSituacao);
        }

        #region Util
        public static void ValidarInformacaoServico(Servico servico)
        {
            if (servico.DataAgendamento == default)
            {
                throw new Exception("A data não pode ser vazia!");
            }
            if (servico.ValorPrevisto <= 0)
            {
                throw new Exception("O valor deve ser maior que zero!");
            }
        }

        public async Task ValidarCliente(int clienteId)
        {
            var cliente = await _clienteRepositorio.Obter(clienteId);
            if (cliente == null)
            {
                throw new Exception($"Cliente {clienteId} não encontrado");
            }
            if (!cliente.Ativo)
            {
                throw new Exception($"Cliente {clienteId} está inativo");
            }
        }

        public async Task ValidarVeiculo(int veiculoId, int clienteId)
        {
            var veiculo = await _veiculoRepositorio.ObterPorId(veiculoId);
            if (veiculo == null)
            {
                throw new Exception($"Veículo {veiculoId} não encontrado");
            }
            if (!veiculo.Ativo)
            {
                throw new Exception($"Veículo {veiculoId} está inativo");
            }
            if (veiculo.ClienteId != clienteId)
            {
                throw new Exception($"Veículo {veiculoId} não pertence ao cliente {clienteId}");
            }
        }
        #endregion
    }
}