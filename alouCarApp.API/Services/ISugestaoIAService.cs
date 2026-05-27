using AlouCar.Dominio.Entidades;
using alouCarApp.API.Models.IA;

namespace alouCarApp.API.Services
{
    public interface ISugestaoIAService
    {
        Task<SugestaoIAResponse> ObterSugestoes(Veiculo veiculo, int kilometragemAtual, List<Servico> historico);
    }
}