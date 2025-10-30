using System.ComponentModel.DataAnnotations;

namespace BibliotecaUniversitaria.Application.ViewModels
{
    public class LivroViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(300, ErrorMessage = "Título deve ter no máximo 300 caracteres")]
        public string Titulo { get; set; } = string.Empty;



        [StringLength(2000, ErrorMessage = "Sinopse deve ter no máximo 2000 caracteres")]
        public string? Sinopse { get; set; }

        [BibliotecaUniversitaria.Application.Attributes.MaxCurrentYear(ErrorMessage = "Ano de publicação não pode ser futuro")]
        public int AnoPublicacao { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantidade total deve ser maior que zero")]
        public int QuantidadeTotal { get; set; }

        [StringLength(200, ErrorMessage = "Editora deve ter no máximo 200 caracteres")]
        public string? Editora { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Número de páginas deve ser maior ou igual a zero")]
        public int NumeroPaginas { get; set; }

        [Required(ErrorMessage = "Autor é obrigatório")]
        public int AutorId { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatória")]
        public int CategoriaId { get; set; }

        public string AutorNome { get; set; } = string.Empty;
        public string CategoriaNome { get; set; } = string.Empty;
        public int QuantidadeDisponivel { get; set; }
    }

    public class LivroListViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string AutorNome { get; set; } = string.Empty;
        public string CategoriaNome { get; set; } = string.Empty;
        public int QuantidadeDisponivel { get; set; }
        public int QuantidadeTotal { get; set; }
        public bool EstaDisponivel => QuantidadeDisponivel > 0;
    }
}
