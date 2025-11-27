using Mapster;
using BibliotecaUniversitaria.Application.DTOs;
using BibliotecaUniversitaria.Application.ViewModels;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Domain.Enums;

namespace BibliotecaUniversitaria.Application.Mappings
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            // Mapeamento Livro -> LivroDTO
            TypeAdapterConfig<Livro, LivroDTO>
                .NewConfig()
                .Map(dest => dest.AutorNome, src => src.Autor != null ? src.Autor.Nome : string.Empty)
                .Map(dest => dest.CategoriaNome, src => src.Categoria != null ? src.Categoria.Nome : string.Empty);

            // Mapeamento Livro -> LivroViewModel
            TypeAdapterConfig<Livro, LivroViewModel>
                .NewConfig()
                .Map(dest => dest.AutorNome, src => src.Autor != null ? src.Autor.Nome : string.Empty)
                .Map(dest => dest.CategoriaNome, src => src.Categoria != null ? src.Categoria.Nome : string.Empty);

            // Mapeamento Livro -> LivroListViewModel
            TypeAdapterConfig<Livro, LivroListViewModel>
                .NewConfig()
                .Map(dest => dest.AutorNome, src => src.Autor != null ? src.Autor.Nome : string.Empty)
                .Map(dest => dest.CategoriaNome, src => src.Categoria != null ? src.Categoria.Nome : string.Empty);

            // Mapeamento Emprestimo -> EmprestimoDTO
            TypeAdapterConfig<Emprestimo, EmprestimoDTO>
                .NewConfig()
                .Map(dest => dest.LivroTitulo, src => src.Livro != null ? src.Livro.Titulo : string.Empty)
                .Map(dest => dest.Status, src => src.Status.ToString());
        }
    }
}

