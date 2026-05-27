using AlouCar.Dominio.Enumeradores;

namespace AlouCar.Dominio.Entidades
{
    public class Veiculo
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Cor { get; set; }
        public int AnoFabricacao { get; set; }
        public int AnoModelo { get; set; }
        public int Quilometragem { get; set; }
        public TipoVeiculo Tipo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? UltimaRevisao { get; set; }
        public bool Ativo { get; set; }
        public Cliente Cliente { get; set; }

        public Veiculo()
        {
            Ativo = true;
            DataCadastro = DateTime.Now;
        }

        public void Deletar()
        {
            Ativo = false;
        }

        public void RegistrarRevisao()
        {
            UltimaRevisao = DateTime.Now;
        }
    }
}