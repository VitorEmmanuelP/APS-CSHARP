using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Domain.Enums;

namespace BibliotecaUniversitaria.Infrastructure.Data.Configurations
{
    public class MultaConfiguration : IEntityTypeConfiguration<Multa>
    {
        public void Configure(EntityTypeBuilder<Multa> builder)
        {
            builder.ToTable("Multas");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.EmprestimoId)
                .IsRequired();

            builder.Property(m => m.Valor)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(m => m.DataVencimento)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(m => m.DataPagamento)
                .HasColumnType("datetime2");

            builder.Property(m => m.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(m => m.Observacoes)
                .HasMaxLength(500);

            builder.HasOne(m => m.Emprestimo)
                .WithMany()
                .HasForeignKey(m => m.EmprestimoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(m => m.EmprestimoId);
            builder.HasIndex(m => m.Status);
            builder.HasIndex(m => m.DataVencimento);
        }
    }
}
