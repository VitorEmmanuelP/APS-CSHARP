using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Domain.Enums;

namespace BibliotecaUniversitaria.Infrastructure.Data.Configurations
{
    public class AutorConfiguration : IEntityTypeConfiguration<Autor>
    {
        public void Configure(EntityTypeBuilder<Autor> builder)
        {
            builder.ToTable("Autores");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Biografia)
                .HasMaxLength(2000);

            builder.Property(a => a.DataNascimento)
                .HasColumnType("date");

            builder.Property(a => a.Nacionalidade)
                .HasMaxLength(100);

            builder.HasMany(a => a.Livros)
                .WithOne(l => l.Autor)
                .HasForeignKey(l => l.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => a.Nome);
        }
    }
}
