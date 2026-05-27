using AlouCar.Aplicacao.Interfaces;
using alouCarApp.API.Models.IA;
using alouCarApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace alouCarApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SugestaoController : ControllerBase
    {
        private readonly ISugestaoIAService _sugestaoIAService;
        private readonly IVeiculoAplicacao _veiculoAplicacao;
        private readonly IServicoAplicacao _servicoAplicacao;

        public SugestaoController(
            ISugestaoIAService sugestaoIAService,
            IVeiculoAplicacao veiculoAplicacao,
            IServicoAplicacao servicoAplicacao)
        {
            _sugestaoIAService = sugestaoIAService;
            _veiculoAplicacao = veiculoAplicacao;
            _servicoAplicacao = servicoAplicacao;
        }

        [HttpPost("ObterSugestoes")]
        public async Task<IActionResult> ObterSugestoes([FromBody] SugestaoIARequest request)
        {
            try
            {
                var veiculo   = await _veiculoAplicacao.ObterPorId(request.VeiculoId);
                var historico = await _servicoAplicacao.ListarPorCliente(veiculo.ClienteId);

                var sugestoes = await _sugestaoIAService.ObterSugestoes(
                    veiculo,
                    request.KilometragemAtual,
                    historico
                );

                return Ok(sugestoes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}