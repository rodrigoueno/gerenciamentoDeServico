using AlouCar.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlouCar.Repositorio.Configuracoes
{
    public class ServicoConfiguracoes : IEntityTypeConfiguration<Servico>
    {
        public void Configure(EntityTypeBuilder<Servico> builder)
        {
            builder.ToTable("Servicos").HasKey(s => s.Id);

            builder.Property(nameof(Servico.Id)).HasColumnName("Id").IsRequired();
            builder.Property(nameof(Servico.ClienteId)).HasColumnName("ClienteId").IsRequired();
            builder.Property(nameof(Servico.VeiculoId)).HasColumnName("VeiculoId").IsRequired();
            builder.Property(nameof(Servico.TipoServico)).HasColumnName("TipoServico").IsRequired();
            builder.Property(nameof(Servico.DataAgendamento)).HasColumnName("DataAgendamento").IsRequired();
            builder.Property(nameof(Servico.Situacao)).HasColumnName("Situacao").IsRequired();
            builder.Property(nameof(Servico.ValorPrevisto)).HasColumnName("ValorPrevisto").HasPrecision(10, 2);
            builder.Property(nameof(Servico.DataCriacao)).HasColumnName("DataCriacao").IsRequired();
            builder.Property(nameof(Servico.DataConclusao)).HasColumnName("DataConclusao");
            builder.Property(nameof(Servico.ValorTotal)).HasColumnName("ValorTotal").HasPrecision(10, 2);
            builder.Property(nameof(Servico.Observacao)).HasColumnName("Observacao").HasMaxLength(500);
            builder.Property(nameof(Servico.Ativo)).HasColumnName("Ativo").IsRequired();

            builder.HasOne(s => s.Cliente)
                   .WithMany()
                   .HasForeignKey(s => s.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Veiculo)
                   .WithMany()
                   .HasForeignKey(s => s.VeiculoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}