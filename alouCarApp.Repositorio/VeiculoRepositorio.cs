using AlouCar.Dominio.Entidades;
using AlouCar.Repositorio.Contexto;
using AlouCar.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlouCar.Repositorio
{
    public class VeiculoRepositorio : IVeiculoRepositorio
    {
        private readonly AlouCarContexto _contexto;

        public VeiculoRepositorio(AlouCarContexto contexto)
        {
            _contexto = contexto;
        }

        public int Criar(Veiculo veiculo)
        {
            _contexto.Veiculos.Add(veiculo);
            _contexto.SaveChanges();
            return veiculo.Id;
        }

        public void Atualizar(Veiculo veiculo)
        {
            _contexto.Veiculos.Update(veiculo);
            _contexto.SaveChanges();
        }

        public async Task<Veiculo> ObterPorId(int id)
        {
            return await _contexto.Veiculos
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<Veiculo>> Listar(bool ativo)
        {
            return await _contexto.Veiculos
                .Where(v => v.Ativo == ativo)
                .ToListAsync();
        }

        public async Task<List<Veiculo>> ListarPorCliente(int clienteId)
        {
            return await _contexto.Veiculos
                .Where(v => v.ClienteId == clienteId && v.Ativo)
                .ToListAsync();
        }

        public async Task<bool> Excluir(Veiculo veiculo)
        {
            veiculo.Deletar();
            _contexto.Veiculos.Update(veiculo);
            await _contexto.SaveChangesAsync();
            return true;
        }
    }
}