using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Domain.Enums;

namespace BibliotecaUniversitaria.Infrastructure.Data.Configurations
{
    public class EmprestimoConfiguration : IEntityTypeConfiguration<Emprestimo>
    {
        public void Configure(EntityTypeBuilder<Emprestimo> builder)
        {
            builder.ToTable("Emprestimos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.LivroId)
                .IsRequired();

            builder.Property(e => e.DataEmprestimo)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(e => e.DataDevolucaoPrevista)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(e => e.DataDevolucaoReal)
                .HasColumnType("datetime2");

            builder.Property(e => e.QuantidadeEmprestada)
                .IsRequired();

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.Observacoes)
                .HasMaxLength(500);

            builder.HasOne(e => e.Livro)
                .WithMany(l => l.Emprestimos)
                .HasForeignKey(e => e.LivroId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.LivroId);
            builder.HasIndex(e => e.DataEmprestimo);
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => e.DataDevolucaoPrevista);
        }
    }
}
