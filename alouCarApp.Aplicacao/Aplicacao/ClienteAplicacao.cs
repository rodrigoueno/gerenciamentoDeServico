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
                throw new Exception("Cliente não pode ser vazio");

            ValidarInformacaoCliente(cliente);
            return _clienteRepositorio.Criar(cliente);
        }

        public void Atualizar(Cliente cliente)
        {
            var clienteDominio = _clienteRepositorio.Obter(cliente.Id);
            if (clienteDominio == null)
                throw new Exception("Cliente não encontrado");

            ValidarInformacaoCliente(cliente);
            clienteDominio.Nome = cliente.Nome;
            clienteDominio.Cpf = cliente.Cpf;
            clienteDominio.Email = cliente.Email;
            clienteDominio.Telefone = cliente.Telefone;
            _clienteRepositorio.Atualizar(clienteDominio);
        }

        public async Task<Cliente> ObterPorId(int id)
        {
            var clienteDominio = _clienteRepositorio.Obter(id);
            if (clienteDominio == null)
                throw new Exception("Cliente não encontrado");
            return clienteDominio;
        }

        public async Task<List<Cliente>> Listar(bool ativo)
        {
            return await _clienteRepositorio.Listar(ativo);
        }

        public async Task<bool> Excluir(Cliente cliente)
        {
            var clienteDominio = _clienteRepositorio.Obter(cliente.Id);
            if (clienteDominio == null)
                throw new Exception("Cliente não encontrado");

            return await _clienteRepositorio.Excluir(clienteDominio);
        }

        #region Util
        private static void ValidarInformacaoCliente(Cliente cliente)
        {
            if (string.IsNullOrEmpty(cliente.Nome))
                throw new Exception("Nome não pode ser vazio");
            if (string.IsNullOrEmpty(cliente.Cpf))
                throw new Exception("CPF não pode ser vazio");
            if (string.IsNullOrEmpty(cliente.Email))
                throw new Exception("Email não pode ser vazio");
            if (string.IsNullOrEmpty(cliente.Telefone))
                throw new Exception("Telefone não pode ser vazio");
        }
        #endregion
    }
}