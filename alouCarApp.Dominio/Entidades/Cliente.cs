namespace AlouCar.Dominio.Entidades
{
    public class Cliente
    {
        
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        public List<Veiculo> Veiculos { get; set; }


        public Cliente()
        {
            Ativo = true;
            DataCadastro = DateTime.Now;
            Veiculos = new List<Veiculo>();
        }

        public void Deletar()
        {
            Ativo = false;
        }

    }
}