using AlouCar.Dominio.Entidades;
using AlouCar.Repositorio.Configuracoes;
using Microsoft.EntityFrameworkCore;

namespace AlouCar.Repositorio.Contexto
{
    public class AlouCarContexto : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Servico> Servicos { get; set; }

        public AlouCarContexto() { }

        public AlouCarContexto(DbContextOptions<AlouCarContexto> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-07MR2BV\\SQLEXPRESS;Database=AlouCar;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteConfiguracoes());
            modelBuilder.ApplyConfiguration(new VeiculoConfiguracoes());
            modelBuilder.ApplyConfiguration(new ServicoConfiguracoes());
        }
    }
}