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
        public IActionResult Criar([FromBody] ServicoCriar servicoCriar)
        {
            try
            {
                var servicoDominio = new Servico
                {
                    ClienteId = servicoCriar.ClienteId,
                    VeiculoId = servicoCriar.VeiculoId,
                    TipoServico = servicoCriar.TipoServico,
                    DataAgendamento = servicoCriar.DataAgendamento,
                    ValorPrevisto = servicoCriar.ValorPrevisto,
                    Observacao = servicoCriar.Observacao
                };
                var servicoId = _servicoAplicacao.Criar(servicoDominio);
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
                var servicoDominio = await _servicoAplicacao.ObterPorId(servicoId);
                var servicoResposta = new ServicoResponse
                {
                    Id = servicoDominio.Id,
                    ClienteId = servicoDominio.ClienteId,
                    NomeCliente = servicoDominio.Cliente?.Nome,
                    VeiculoId = servicoDominio.VeiculoId,
                    ModeloVeiculo = servicoDominio.Veiculo?.Modelo,
                    PlacaVeiculo = servicoDominio.Veiculo?.Placa,
                    TipoServico = servicoDominio.TipoServico,
                    DataAgendamento = servicoDominio.DataAgendamento,
                    Situacao = servicoDominio.Situacao,
                    ValorPrevisto = servicoDominio.ValorPrevisto,
                    ValorTotal = servicoDominio.ValorTotal,
                    Observacao = servicoDominio.Observacao,
                    Ativo = servicoDominio.Ativo,
                    DataCriacao = servicoDominio.DataCriacao,
                    DataConclusao = servicoDominio.DataConclusao
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
                var servicosDominio = await _servicoAplicacao.Listar(ativo);
                var servicos = servicosDominio.Select(s => new ServicoResponse
                {
                    Id = s.Id,
                    ClienteId = s.ClienteId,
                    NomeCliente = s.Cliente?.Nome,
                    VeiculoId = s.VeiculoId,
                    ModeloVeiculo = s.Veiculo?.Modelo,
                    PlacaVeiculo = s.Veiculo?.Placa,
                    TipoServico = s.TipoServico,
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
                    TipoServico = s.TipoServico,
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
        public IActionResult Atualizar([FromRoute] int servicoId, [FromBody] ServicoAtualizar servicoAtualizar)
        {
            try
            {
                var servicoDominio = new Servico
                {
                    Id = servicoId,
                    TipoServico = servicoAtualizar.TipoServico,
                    DataAgendamento = servicoAtualizar.DataAgendamento,
                    Situacao = servicoAtualizar.Situacao,
                    ValorPrevisto = servicoAtualizar.ValorPrevisto,
                    ValorTotal = servicoAtualizar.ValorTotal,
                    Observacao = servicoAtualizar.Observacao
                };
                _servicoAplicacao.Atualizar(servicoDominio);
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
                var servicoDominio = await _servicoAplicacao.ObterPorId(servicoId);
                await _servicoAplicacao.Excluir(servicoDominio);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}