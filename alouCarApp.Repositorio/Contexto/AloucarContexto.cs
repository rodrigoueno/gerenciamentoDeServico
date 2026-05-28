using AlouCar.Dominio.Entidades;
using AlouCar.Repositorio.Configuracoes;
using Microsoft.EntityFrameworkCore;

namespace AlouCar.Repositorio.Contexto
{
    public class AlouCarContexto : DbContext
    {
        private readonly DbContextOptions _options;
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<ServicoItem> ServicoItens { get; set; }

        public AlouCarContexto() { }

        public AlouCarContexto(DbContextOptions<AlouCarContexto> options) : base(options)
        {
            _options = options;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_options == null)
            {
                optionsBuilder.UseSqlServer("Server=NOTE291\\SQLEXPRESS;Database=AlouCar;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteConfiguracoes());
            modelBuilder.ApplyConfiguration(new VeiculoConfiguracoes());
            modelBuilder.ApplyConfiguration(new ServicoConfiguracoes());
            modelBuilder.ApplyConfiguration(new ServicoItemConfiguracoes()); // <- novo
        }
    }
}