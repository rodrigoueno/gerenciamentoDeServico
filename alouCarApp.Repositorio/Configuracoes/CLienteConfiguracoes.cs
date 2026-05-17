using AlouCar.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlouCar.Repositorio
{
    public class ClienteConfiguracoes : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes").HasKey(c => c.Id);

            builder.Property(nameof(Cliente.Id)).HasColumnName("Id").IsRequired();
            builder.Property(nameof(Cliente.Nome)).HasColumnName("Nome").IsRequired().HasMaxLength(50);
            builder.Property(nameof(Cliente.Cpf)).HasColumnName("CPF").IsRequired().HasMaxLength(14);
            builder.Property(nameof(Cliente.Email)).HasColumnName("Email").IsRequired().HasMaxLength(50);
            builder.Property(nameof(Cliente.Telefone)).HasColumnName("Telefone").HasMaxLength(15);
            builder.Property(nameof(Cliente.Ativo)).HasColumnName("Ativo").IsRequired();
            builder.Property(nameof(Cliente.DataCadastro)).HasColumnName("DataCadastro").IsRequired();

            builder.HasMany(c => c.Veiculos)
                   .WithOne(v => v.Cliente)
                   .HasForeignKey(v => v.ClienteId)
                   .OnDelete(DeleteBehavior.Cascade);
        }   
    }
}