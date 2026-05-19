using AlouCar.Dominio.Enumeradores;

namespace alouCarApp.API.Models.Veiculos
{
    public class VeiculoAtualizar
    {
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Cor { get; set; }
        public int AnoFabricacao { get; set; }
        public int AnoModelo { get; set; }
        public int Quilometragem { get; set; }
        public TipoVeiculo Tipo { get; set; }
    }
}