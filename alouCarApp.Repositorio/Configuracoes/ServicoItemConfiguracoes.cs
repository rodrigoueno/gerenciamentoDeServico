
using AlouCar.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlouCar.Repositorio.Configuracoes
{
    public class ServicoItemConfiguracoes : IEntityTypeConfiguration<ServicoItem>
    {
        public void Configure(EntityTypeBuilder<ServicoItem> builder)
        {
            builder.ToTable("ServicoItens").HasKey(i => i.Id);

            builder.Property(nameof(ServicoItem.Id)).HasColumnName("Id").IsRequired();
            builder.Property(nameof(ServicoItem.ServicoId)).HasColumnName("ServicoId").IsRequired();
            builder.Property(nameof(ServicoItem.TipoServico)).HasColumnName("TipoServico").IsRequired();
            builder.Property(nameof(ServicoItem.KilometragemNaRevisao)).HasColumnName("KilometragemNaRevisao").IsRequired();
            builder.Property(nameof(ServicoItem.Valor)).HasColumnName("Valor").HasPrecision(10, 2).IsRequired();

            builder.HasOne(i => i.Servico)
                   .WithMany(s => s.Itens)
                   .HasForeignKey(i => i.ServicoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}