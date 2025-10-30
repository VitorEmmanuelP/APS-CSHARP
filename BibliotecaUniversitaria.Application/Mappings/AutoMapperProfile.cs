using AutoMapper;
using BibliotecaUniversitaria.Application.DTOs;
using BibliotecaUniversitaria.Application.ViewModels;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Domain.Enums;

namespace BibliotecaUniversitaria.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Livro, LivroDTO>()
                .ForMember(dest => dest.AutorNome, opt => opt.MapFrom(src => src.Autor != null ? src.Autor.Nome : string.Empty))
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nome : string.Empty));

            CreateMap<Livro, LivroViewModel>()
                .ForMember(dest => dest.AutorNome, opt => opt.MapFrom(src => src.Autor != null ? src.Autor.Nome : string.Empty))
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nome : string.Empty));

            CreateMap<Livro, LivroListViewModel>()
                .ForMember(dest => dest.AutorNome, opt => opt.MapFrom(src => src.Autor != null ? src.Autor.Nome : string.Empty))
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nome : string.Empty));


            CreateMap<Emprestimo, EmprestimoDTO>()
                .ForMember(dest => dest.LivroTitulo, opt => opt.MapFrom(src => src.Livro != null ? src.Livro.Titulo : string.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Autor, DTOs.AutorDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Biografia, opt => opt.MapFrom(src => src.Biografia))
                .ForMember(dest => dest.DataNascimento, opt => opt.MapFrom(src => src.DataNascimento))
                .ForMember(dest => dest.Nacionalidade, opt => opt.MapFrom(src => src.Nacionalidade));

            CreateMap<Autor, DTOs.AutorCreateDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Biografia, opt => opt.MapFrom(src => src.Biografia))
                .ForMember(dest => dest.DataNascimento, opt => opt.MapFrom(src => src.DataNascimento))
                .ForMember(dest => dest.Nacionalidade, opt => opt.MapFrom(src => src.Nacionalidade));

            CreateMap<Categoria, DTOs.CategoriaDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));

            CreateMap<Categoria, DTOs.CategoriaCreateDTO>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));

        }
    }
}
