using AlouCar.Aplicacao.Interfaces;
using AlouCar.Dominio.Entidades;
using AlouCar.Repositorio.Interfaces;

namespace AlouCar.Aplicacao.Aplicacoes
{
    public class ClienteAplicacao : IClienteAplicacao
    {
        readonly IClienteRepositorio _clienteRepositorio;

        public ClienteAplicacao(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public int Criar(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new Exception("Cliente não pode ser vazio");
            }

            ValidarInformacaoCliente(cliente);
            return _clienteRepositorio.Criar(cliente);
        }
        
        public async Task Atualizar(Cliente cliente)
        {
            var clienteAtualizar = await _clienteRepositorio.Obter(cliente.Id);
            if (clienteAtualizar == null)
                throw new Exception("Cliente não encontrado");

            if (!string.IsNullOrEmpty(cliente.Nome))
            {
                clienteAtualizar.Nome = cliente.Nome;
            }
            if (!string.IsNullOrEmpty(cliente.Cpf))
            {
                clienteAtualizar.Cpf = cliente.Cpf;
            }
            if (!string.IsNullOrEmpty(cliente.Email))
            {
                clienteAtualizar.Email = cliente.Email;
            }
            if (!string.IsNullOrEmpty(cliente.Telefone))
            {
                clienteAtualizar.Telefone = cliente.Telefone;
            }

            _clienteRepositorio.Atualizar(clienteAtualizar);
        }

        public async Task<Cliente> ObterPorId(int id)
        {
            var clienteObter = await _clienteRepositorio.Obter(id);
            if (clienteObter == null)
            {
                throw new Exception("Cliente não encontrado");
            }
            else
            {
                return clienteObter;
            }
        }

        public async Task<List<Cliente>> Listar(bool ativo)
        {
            return await _clienteRepositorio.Listar(ativo);
        }

        public async Task<bool> Excluir(Cliente cliente)
        {
            var clienteExcluir = await _clienteRepositorio.Obter(cliente.Id);
            if (clienteExcluir == null)
            {
                throw new Exception("Cliente não encontrado");
            }
            return await _clienteRepositorio.Excluir(clienteExcluir);
        }

        #region Util
        private static void ValidarInformacaoCliente(Cliente cliente)
        {
            if (string.IsNullOrEmpty(cliente.Nome))
            {
                throw new Exception("Nome não pode ser vazio");
            }
            if (string.IsNullOrEmpty(cliente.Cpf))
            {
                throw new Exception("CPF não pode ser vazio");
            }
            if (string.IsNullOrEmpty(cliente.Email))
            {
                throw new Exception("Email não pode ser vazio");
            }
            if (string.IsNullOrEmpty(cliente.Telefone))
            {
                throw new Exception("Telefone não pode ser vazio");
            }
        }
        #endregion
    }
}