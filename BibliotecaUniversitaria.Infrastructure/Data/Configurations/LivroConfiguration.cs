using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BibliotecaUniversitaria.Domain.Entities;

namespace BibliotecaUniversitaria.Infrastructure.Data.Configurations
{
    public class LivroConfiguration : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.ToTable("Livros");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Titulo)
                .IsRequired()
                .HasMaxLength(300);


            builder.Property(l => l.Sinopse)
                .HasMaxLength(2000);

            builder.Property(l => l.AnoPublicacao)
                .IsRequired();

            builder.Property(l => l.QuantidadeDisponivel)
                .IsRequired();

            builder.Property(l => l.QuantidadeTotal)
                .IsRequired();

            builder.Property(l => l.Editora)
                .HasMaxLength(200);

            builder.Property(l => l.NumeroPaginas)
                .IsRequired();

            builder.HasOne(l => l.Autor)
                .WithMany(a => a.Livros)
                .HasForeignKey(l => l.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Categoria)
                .WithMany(c => c.Livros)
                .HasForeignKey(l => l.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(l => l.Emprestimos)
                .WithOne(e => e.Livro)
                .HasForeignKey(e => e.LivroId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(l => l.Titulo);
            builder.HasIndex(l => l.AutorId);
            builder.HasIndex(l => l.CategoriaId);
        }
    }
}
