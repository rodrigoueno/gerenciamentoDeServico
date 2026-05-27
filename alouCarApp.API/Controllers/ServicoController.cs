using Microsoft.AspNetCore.Mvc;
using AlouCar.Aplicacao.Interfaces;
using AlouCar.Dominio.Entidades;
using alouCarApp.API.Models.Servicos.Resposta;
using alouCarApp.API.Models.Servicos;

namespace alouCarApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicoController : ControllerBase
    {
        private readonly IServicoAplicacao _servicoAplicacao;

        public ServicoController(IServicoAplicacao servicoAplicacao)
        {
            _servicoAplicacao = servicoAplicacao;
        }

        [HttpPost("Criar")]
        public async Task<IActionResult> Criar([FromBody] ServicoCriar servicoCriar)
        {
            try
            {
                var servico = new Servico
                {
                    ClienteId = servicoCriar.ClienteId,
                    VeiculoId = servicoCriar.VeiculoId,
                    DataAgendamento = servicoCriar.DataAgendamento,
                    Observacao = servicoCriar.Observacao,
                    Itens = servicoCriar.Itens.Select(i => new ServicoItem
                    {
                        TipoServico = i.TipoServico,
                        Valor = i.Valor
                    }).ToList()
                };

                var servicoId = await _servicoAplicacao.Criar(servico);
                return Ok(servicoId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Obter/{servicoId}")]
        public async Task<IActionResult> Obter([FromRoute] int servicoId)
        {
            try
            {
                var servicoObter = await _servicoAplicacao.ObterPorId(servicoId);
                var servicoResposta = new ServicoResponse
                {
                    Id = servicoObter.Id,
                    ClienteId = servicoObter.ClienteId,
                    NomeCliente = servicoObter.Cliente?.Nome,
                    VeiculoId = servicoObter.VeiculoId,
                    ModeloVeiculo = servicoObter.Veiculo?.Modelo,
                    PlacaVeiculo = servicoObter.Veiculo?.Placa,
                    Itens = servicoObter.Itens?.Select(i => new ServicoItemResposta
                    {
                        Id = i.Id,
                        TipoServico = i.TipoServico,
                        Valor = i.Valor
                    }).ToList() ?? new(),
                    DataAgendamento = servicoObter.DataAgendamento,
                    Situacao = servicoObter.Situacao,
                    ValorPrevisto = servicoObter.ValorPrevisto,
                    ValorTotal = servicoObter.ValorTotal,
                    Observacao = servicoObter.Observacao,
                    Ativo = servicoObter.Ativo,
                    DataCriacao = servicoObter.DataCriacao,
                    DataConclusao = servicoObter.DataConclusao
                };
                return Ok(servicoResposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar([FromQuery] bool ativo)
        {
            try
            {
                var servicosListar = await _servicoAplicacao.Listar(ativo);
                var servicos = servicosListar.Select(s => new ServicoResponse
                {
                    Id = s.Id,
                    ClienteId = s.ClienteId,
                    NomeCliente = s.Cliente?.Nome,
                    VeiculoId = s.VeiculoId,
                    ModeloVeiculo = s.Veiculo?.Modelo,
                    PlacaVeiculo = s.Veiculo?.Placa,
                    Itens = s.Itens?.Select(i => new ServicoItemResposta
                    {
                        Id = i.Id,
                        TipoServico = i.TipoServico,
                        Valor = i.Valor
                    }).ToList() ?? new(),
                    DataAgendamento = s.DataAgendamento,
                    Situacao = s.Situacao,
                    ValorPrevisto = s.ValorPrevisto,
                    ValorTotal = s.ValorTotal,
                    Ativo = s.Ativo,
                    DataCriacao = s.DataCriacao
                }).ToList();
                return Ok(servicos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ListarPorCliente/{clienteId}")]
        public async Task<IActionResult> ListarPorCliente([FromRoute] int clienteId)
        {
            try
            {
                var servicosDominio = await _servicoAplicacao.ListarPorCliente(clienteId);
                var servicos = servicosDominio.Select(s => new ServicoResponse
                {
                    Id = s.Id,
                    ClienteId = s.ClienteId,
                    VeiculoId = s.VeiculoId,
                    ModeloVeiculo = s.Veiculo?.Modelo,
                    PlacaVeiculo = s.Veiculo?.Placa,
                    Itens = s.Itens?.Select(i => new ServicoItemResposta
                    {
                        Id = i.Id,
                        TipoServico = i.TipoServico,
                        Valor = i.Valor
                    }).ToList() ?? new(),
                    DataAgendamento = s.DataAgendamento,
                    Situacao = s.Situacao,
                    ValorPrevisto = s.ValorPrevisto,
                    ValorTotal = s.ValorTotal,
                    Ativo = s.Ativo
                }).ToList();
                return Ok(servicos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Atualizar/{servicoId}")]
        public async Task<IActionResult> Atualizar([FromRoute] int servicoId, [FromBody] ServicoAtualizar servicoAtualizar)
        {
            try
            {
                var servico = new Servico
                {
                    Id = servicoId,
                    DataAgendamento = servicoAtualizar.DataAgendamento,
                    Situacao = servicoAtualizar.Situacao,
                    ValorTotal = servicoAtualizar.ValorTotal,
                    Observacao = servicoAtualizar.Observacao
                };
                await _servicoAplicacao.Atualizar(servico);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            // catch (Exception ex)
            // {
            //     return BadRequest(ex.Message);
            // }
        }

        [HttpPut("AtualizarSituacao/{servicoId}")]
        public IActionResult AtualizarSituacao([FromRoute] int servicoId, [FromBody] ServicoAtualizarSituacao model)
        {
            try
            {
                _servicoAplicacao.AtualizarSituacao(servicoId, model.Situacao);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Excluir/{servicoId}")]
        public async Task<IActionResult> Excluir([FromRoute] int servicoId)
        {
            try
            {
                var servicoExcluir = await _servicoAplicacao.ObterPorId(servicoId);
                await _servicoAplicacao.Excluir(servicoExcluir);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}