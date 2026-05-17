using AlouCar.Aplicacao.Interfaces;
using AlouCar.Dominio.Entidades;
using AlouCar.Repositorio.Interfaces;

namespace AlouCar.Aplicacao.Aplicacoes
{
    public class VeiculoAplicacao : IVeiculoAplicacao
    {
        readonly IVeiculoRepositorio _veiculoRepositorio;
        readonly IClienteRepositorio _clienteRepositorio;

        public VeiculoAplicacao(IVeiculoRepositorio veiculoRepositorio, IClienteRepositorio clienteRepositorio)
        {
            _veiculoRepositorio = veiculoRepositorio;
            _clienteRepositorio = clienteRepositorio;
        }

        public int Criar(Veiculo veiculo)
        {
            if (veiculo == null)
                throw new Exception("Veículo não pode ser vazio");

            ValidarInformacaoVeiculo(veiculo);
            ValidarCliente(veiculo.ClienteId);
            return _veiculoRepositorio.Criar(veiculo);
        }

        public void Atualizar(Veiculo veiculo)
        {
            var veiculoDominio = _veiculoRepositorio.ObterPorId(veiculo.Id).Result;
            if (veiculoDominio == null)
                throw new Exception("Veículo não encontrado");

            ValidarInformacaoVeiculo(veiculo);
            veiculoDominio.Placa = veiculo.Placa;
            veiculoDominio.Marca = veiculo.Marca;
            veiculoDominio.Modelo = veiculo.Modelo;
            veiculoDominio.Cor = veiculo.Cor;
            veiculoDominio.AnoFabricacao = veiculo.AnoFabricacao;
            veiculoDominio.AnoModelo = veiculo.AnoModelo;
            veiculoDominio.Quilometragem = veiculo.Quilometragem;
            veiculoDominio.Tipo = veiculo.Tipo;
            _veiculoRepositorio.Atualizar(veiculoDominio);
        }

        public async Task<Veiculo> ObterPorId(int id)
        {
            var veiculoDominio = await _veiculoRepositorio.ObterPorId(id);
            if (veiculoDominio == null)
                throw new Exception("Veículo não encontrado");
            return veiculoDominio;
        }

        public async Task<List<Veiculo>> Listar(bool ativo)
        {
            return await _veiculoRepositorio.Listar(ativo);
        }

        public async Task<List<Veiculo>> ListarPorCliente(int clienteId)
        {
            return await _veiculoRepositorio.ListarPorCliente(clienteId);
        }

        public async Task<bool> Excluir(Veiculo veiculo)
        {
            var veiculoDominio = await _veiculoRepositorio.ObterPorId(veiculo.Id);
            if (veiculoDominio == null)
                throw new Exception("Veículo não encontrado");

            return await _veiculoRepositorio.Excluir(veiculoDominio);
        }

        #region Util
        private static void ValidarInformacaoVeiculo(Veiculo veiculo)
        {
            if (string.IsNullOrEmpty(veiculo.Placa))
                throw new Exception("Placa não pode ser vazia");
            if (string.IsNullOrEmpty(veiculo.Marca))
                throw new Exception("Marca não pode ser vazia");
            if (string.IsNullOrEmpty(veiculo.Modelo))
                throw new Exception("Modelo não pode ser vazio");
            if (veiculo.AnoFabricacao <= 0)
                throw new Exception("Ano de fabricação inválido");
            if (veiculo.AnoModelo <= 0)
                throw new Exception("Ano do modelo inválido");
        }

        private void ValidarCliente(int clienteId)
        {
            var cliente = _clienteRepositorio.Obter(clienteId);
            if (cliente == null)
                throw new Exception($"Cliente {clienteId} não encontrado");
            if (!cliente.Ativo)
                throw new Exception($"Cliente {clienteId} está inativo");
        }
        #endregion
    }
}