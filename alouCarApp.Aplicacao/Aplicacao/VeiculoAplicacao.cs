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
            {
                throw new Exception("Veículo não pode ser vazio");
            }

            ValidarInformacaoVeiculo(veiculo);
            ValidarCliente(veiculo.ClienteId);
            return _veiculoRepositorio.Criar(veiculo);
        }

        public void Atualizar(Veiculo veiculo)
        {
            var veiculoAtualizar = _veiculoRepositorio.ObterPorId(veiculo.Id).Result;
            if (veiculoAtualizar == null)
                throw new Exception("Veículo não encontrado");

            if (!string.IsNullOrEmpty(veiculo.Placa))
            {
                veiculoAtualizar.Placa = veiculo.Placa;
            }
            if (!string.IsNullOrEmpty(veiculo.Marca))
            {
                veiculoAtualizar.Marca = veiculo.Marca;
            }
            if (!string.IsNullOrEmpty(veiculo.Modelo))
            {
                veiculoAtualizar.Modelo = veiculo.Modelo;
            }
            if (!string.IsNullOrEmpty(veiculo.Cor))
            {
                veiculoAtualizar.Cor = veiculo.Cor;
            }
            if (veiculo.AnoFabricacao > 0)
            {
                veiculoAtualizar.AnoFabricacao = veiculo.AnoFabricacao;
            }
            if (veiculo.AnoModelo > 0)
            {
                veiculoAtualizar.AnoModelo = veiculo.AnoModelo;
            }
            if (veiculo.Quilometragem > 0)
            {
                veiculoAtualizar.Quilometragem = veiculo.Quilometragem;
            }
            if (veiculo.Tipo != default)
            {
                veiculoAtualizar.Tipo = veiculo.Tipo;
            }
            _veiculoRepositorio.Atualizar(veiculoAtualizar);
        }

        public async Task<Veiculo> ObterPorId(int id)
        {
            var veiculoObter = await _veiculoRepositorio.ObterPorId(id);
            if (veiculoObter == null)
            {
                throw new Exception("Veículo não encontrado");
            }
            return veiculoObter;
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
            var veiculoExcluir = await _veiculoRepositorio.ObterPorId(veiculo.Id);
            if (veiculoExcluir == null)
            {
                throw new Exception("Veículo não encontrado");
            }

            return await _veiculoRepositorio.Excluir(veiculoExcluir);

        }

        #region Util
        public static void ValidarInformacaoVeiculo(Veiculo veiculo)
        {
            if (string.IsNullOrEmpty(veiculo.Placa))
            {
                throw new Exception("Placa não pode ser vazia");
            }
            if (string.IsNullOrEmpty(veiculo.Marca))
            {
                throw new Exception("Marca não pode ser vazia");
            }
            if (string.IsNullOrEmpty(veiculo.Modelo))
            {
                throw new Exception("Modelo não pode ser vazio");
            }
            if (veiculo.AnoFabricacao <= 0)
            {
                throw new Exception("Ano de fabricação inválido");
            }
            if (veiculo.AnoModelo <= 0)
            {
                throw new Exception("Ano do modelo inválido");
            }
        }

        public async void ValidarCliente(int clienteId)
        {
            var cliente = await _clienteRepositorio.Obter(clienteId);
            if (cliente == null)
            {
                throw new Exception($"Cliente {clienteId} não encontrado");
            }
            if (!cliente.Ativo)
            {
                throw new Exception($"Cliente {clienteId} está inativo");
            }
        }
        #endregion
    }
}