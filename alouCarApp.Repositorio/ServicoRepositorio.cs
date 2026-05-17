using AlouCar.Dominio.Entidades;
using AlouCar.Repositorio.Contexto;
using AlouCar.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlouCar.Repositorio
{
    public class ServicoRepositorio : IServicoRepositorio
    {
        private readonly AlouCarContexto _contexto;

        public ServicoRepositorio(AlouCarContexto contexto)
        {
            _contexto = contexto;
        }

        public int Criar(Servico servico)
        {
            _contexto.Servicos.Add(servico);
            _contexto.SaveChanges();
            return servico.Id;
        }

        public void Atualizar(Servico servico)
        {
            _contexto.Servicos.Update(servico);
            _contexto.SaveChanges();
        }

        public async Task<Servico> ObterPorId(int id)
        {
            return await _contexto.Servicos
                .Include(s => s.Cliente)
                .Include(s => s.Veiculo)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Servico>> Listar(bool ativo)
        {
            return await _contexto.Servicos
                .Where(s => s.Ativo == ativo)
                .Include(s => s.Cliente)
                .Include(s => s.Veiculo)
                .ToListAsync();
        }

        public async Task<List<Servico>> ListarPorCliente(int clienteId)
        {
            return await _contexto.Servicos
                .Where(s => s.ClienteId == clienteId && s.Ativo)
                .Include(s => s.Veiculo)
                .ToListAsync();
        }

        public async Task<bool> Excluir(Servico servico)
        {
            servico.Deletar();
            _contexto.Servicos.Update(servico);
            await _contexto.SaveChangesAsync();
            return true;
        }
    }
}