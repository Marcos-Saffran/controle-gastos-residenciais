using Gastos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gastos.Infrastructure.EF.Configurations;

public class TransacaoConfiguration : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.ToTable("Transacoes");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
               .ValueGeneratedOnAdd();

        builder.Property(t => t.Descricao)
               .IsRequired()
               .HasMaxLength(400);

        builder.Property(t => t.Valor)
               .IsRequired()
               .HasColumnType("decimal(10,2)");

        builder.Property(t => t.Tipo)
               .IsRequired();

        // Relacionamento com Categoria
        builder.HasOne<Categoria>()
               .WithMany()
               .HasForeignKey(t => t.CategoriaId)
               .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Pessoa
        builder.HasOne<Pessoa>()
               .WithMany()
               .HasForeignKey(t => t.PessoaId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}