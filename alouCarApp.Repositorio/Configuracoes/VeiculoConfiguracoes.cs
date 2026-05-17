using AlouCar.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlouCar.Repositorio
{
    public class VeiculoConfiguracoes : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.ToTable("Veiculos").HasKey(v => v.Id);

            builder.Property(nameof(Veiculo.Id)).HasColumnName("Id").IsRequired();
            builder.Property(nameof(Veiculo.ClienteId)).HasColumnName("ClienteId").IsRequired();
            builder.Property(nameof(Veiculo.Placa)).HasColumnName("Placa").IsRequired().HasMaxLength(10);
            builder.Property(nameof(Veiculo.Marca)).HasColumnName("Marca").IsRequired().HasMaxLength(50);
            builder.Property(nameof(Veiculo.Modelo)).HasColumnName("Modelo").IsRequired().HasMaxLength(50);
            builder.Property(nameof(Veiculo.Cor)).HasColumnName("Cor").HasMaxLength(30);
            builder.Property(nameof(Veiculo.AnoFabricacao)).HasColumnName("AnoFabricacao").IsRequired();
            builder.Property(nameof(Veiculo.AnoModelo)).HasColumnName("AnoModelo").IsRequired();
            builder.Property(nameof(Veiculo.Quilometragem)).HasColumnName("Quilometragem");
            builder.Property(nameof(Veiculo.Tipo)).HasColumnName("Tipo").IsRequired();
            builder.Property(nameof(Veiculo.DataCadastro)).HasColumnName("DataCadastro").IsRequired();
            builder.Property(nameof(Veiculo.UltimaRevisao)).HasColumnName("UltimaRevisao");
            builder.Property(nameof(Veiculo.Ativo)).HasColumnName("Ativo").IsRequired();
        }
    }
}