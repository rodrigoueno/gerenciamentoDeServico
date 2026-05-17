using Microsoft.AspNetCore.Mvc;
using AlouCar.Aplicacao.Interfaces;
using AlouCar.Dominio.Entidades;
using alouCarApp.API.Models.Veiculos.Resposta;
using alouCarApp.API.Models;

namespace alouCarApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VeiculoController : ControllerBase
    {
        private readonly IVeiculoAplicacao _veiculoAplicacao;

        public VeiculoController(IVeiculoAplicacao veiculoAplicacao)
        {
            _veiculoAplicacao = veiculoAplicacao;
        }

        [HttpPost("Criar")]
        public IActionResult Criar([FromBody] VeiculoCriar veiculoCriar)
        {
            try
            {
                var veiculoDominio = new Veiculo
                {
                    ClienteId = veiculoCriar.ClienteId,
                    Placa = veiculoCriar.Placa,
                    Marca = veiculoCriar.Marca,
                    Modelo = veiculoCriar.Modelo,
                    Cor = veiculoCriar.Cor,
                    AnoFabricacao = veiculoCriar.AnoFabricacao,
                    AnoModelo = veiculoCriar.AnoModelo,
                    Quilometragem = veiculoCriar.Quilometragem,
                    Tipo = veiculoCriar.Tipo
                };
                var veiculoId = _veiculoAplicacao.Criar(veiculoDominio);
                return Ok(veiculoId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Obter/{veiculoId}")]
        public async Task<IActionResult> Obter([FromRoute] int veiculoId)
        {
            try
            {
                var veiculoDominio = await _veiculoAplicacao.ObterPorId(veiculoId);
                var veiculoResposta = new VeiculoResponse
                {
                    Id = veiculoDominio.Id,
                    ClienteId = veiculoDominio.ClienteId,
                    Placa = veiculoDominio.Placa,
                    Marca = veiculoDominio.Marca,
                    Modelo = veiculoDominio.Modelo,
                    Cor = veiculoDominio.Cor,
                    AnoFabricacao = veiculoDominio.AnoFabricacao,
                    AnoModelo = veiculoDominio.AnoModelo,
                    Quilometragem = veiculoDominio.Quilometragem,
                    Tipo = veiculoDominio.Tipo,
                    Ativo = veiculoDominio.Ativo,
                    DataCadastro = veiculoDominio.DataCadastro
                };
                return Ok(veiculoResposta);
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
                var veiculosDominio = await _veiculoAplicacao.Listar(ativo);
                var veiculos = veiculosDominio.Select(v => new VeiculoResponse
                {
                    Id = v.Id,
                    ClienteId = v.ClienteId,
                    Placa = v.Placa,
                    Marca = v.Marca,
                    Modelo = v.Modelo,
                    Cor = v.Cor,
                    AnoFabricacao = v.AnoFabricacao,
                    AnoModelo = v.AnoModelo,
                    Quilometragem = v.Quilometragem,
                    Tipo = v.Tipo,
                    Ativo = v.Ativo,
                    DataCadastro = v.DataCadastro
                }).ToList();
                return Ok(veiculos);
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
                var veiculosDominio = await _veiculoAplicacao.ListarPorCliente(clienteId);
                var veiculos = veiculosDominio.Select(v => new VeiculoResponse
                {
                    Id = v.Id,
                    ClienteId = v.ClienteId,
                    Placa = v.Placa,
                    Marca = v.Marca,
                    Modelo = v.Modelo,
                    Cor = v.Cor,
                    AnoFabricacao = v.AnoFabricacao,
                    AnoModelo = v.AnoModelo,
                    Quilometragem = v.Quilometragem,
                    Tipo = v.Tipo,
                    Ativo = v.Ativo
                }).ToList();
                return Ok(veiculos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Atualizar")]
        public IActionResult Atualizar([FromBody] VeiculoAtualizar veiculoAtualizar)
        {
            try
            {
                var veiculoDominio = new Veiculo
                {
                    Id = veiculoAtualizar.Id,
                    Placa = veiculoAtualizar.Placa,
                    Marca = veiculoAtualizar.Marca,
                    Modelo = veiculoAtualizar.Modelo,
                    Cor = veiculoAtualizar.Cor,
                    AnoFabricacao = veiculoAtualizar.AnoFabricacao,
                    AnoModelo = veiculoAtualizar.AnoModelo,
                    Quilometragem = veiculoAtualizar.Quilometragem,
                    Tipo = veiculoAtualizar.Tipo
                };
                _veiculoAplicacao.Atualizar(veiculoDominio);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Excluir/{veiculoId}")]
        public async Task<IActionResult> Excluir([FromRoute] int veiculoId)
        {
            try
            {
                var veiculoDominio = await _veiculoAplicacao.ObterPorId(veiculoId);
                await _veiculoAplicacao.Excluir(veiculoDominio);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}