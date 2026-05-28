using Microsoft.AspNetCore.Mvc;
using AlouCar.Aplicacao.Interfaces;
using AlouCar.Dominio.Entidades;
using alouCarApp.API.Models.Clientes.Resposta;
using alouCarApp.API.Models.Clientes;

namespace alouCarApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteAplicacao _clienteAplicacao;

        public ClienteController(IClienteAplicacao clienteAplicacao)
        {
            _clienteAplicacao = clienteAplicacao;
        }

        [HttpPost("Criar")]
        public IActionResult Criar([FromBody] ClienteCriar clienteCriar)
        {
            try
            {
                var clienteDominio = new Cliente
                {
                    Nome = clienteCriar.Nome,
                    Cpf = clienteCriar.Cpf,
                    Email = clienteCriar.Email,
                    Telefone = clienteCriar.Telefone
                };
                var clienteId = _clienteAplicacao.Criar(clienteDominio);
                return Ok(clienteId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Obter/{clienteId}")]
        public async Task<IActionResult> Obter([FromRoute] int clienteId)
        {
            try
            {
                var clienteDominio = await _clienteAplicacao.ObterPorId(clienteId);
                var clienteResposta = new ClienteResponse
                {
                    Id = clienteDominio.Id,
                    Nome = clienteDominio.Nome,
                    Cpf = clienteDominio.Cpf,
                    Email = clienteDominio.Email,
                    Telefone = clienteDominio.Telefone,
                    Ativo = clienteDominio.Ativo,
                    DataCadastro = clienteDominio.DataCadastro
                };
                return Ok(clienteResposta);
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
                var clientesDominio = await _clienteAplicacao.Listar(ativo);
                var clientes = clientesDominio.Select(c => new ClienteResponse
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Cpf = c.Cpf,
                    Email = c.Email,
                    Telefone = c.Telefone,
                    Ativo = c.Ativo,
                    DataCadastro = c.DataCadastro
                }).ToList();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Atualizar/{clienteId}")]
        public async Task<IActionResult> Atualizar([FromRoute] int clienteId, [FromBody] ClienteAtualizar clienteAtualizar)
        {
            try
            {
                var clienteDominio = new Cliente
                {
                    Id = clienteId,
                    Nome = clienteAtualizar.Nome,
                    Cpf = clienteAtualizar.Cpf,
                    Email = clienteAtualizar.Email,
                    Telefone = clienteAtualizar.Telefone
                };
                await _clienteAplicacao.Atualizar(clienteDominio);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Excluir/{clienteId}")]
        public async Task<IActionResult> Excluir([FromRoute] int clienteId)
        {
            try
            {
                var clienteDominio = await _clienteAplicacao.ObterPorId(clienteId);
                await _clienteAplicacao.Excluir(clienteDominio);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}