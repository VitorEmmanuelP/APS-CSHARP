using System.ComponentModel.DataAnnotations;

namespace BibliotecaUniversitaria.Application.DTOs
{
    public class LivroDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Sinopse { get; set; } = string.Empty;
        public int AnoPublicacao { get; set; }
        public int QuantidadeDisponivel { get; set; }
        public int QuantidadeTotal { get; set; }
        public string Editora { get; set; } = string.Empty;
        public int NumeroPaginas { get; set; }
        public int AutorId { get; set; }
        public string AutorNome { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;
    }

    public class LivroCreateDTO
    {
        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(300, ErrorMessage = "Título deve ter no máximo 300 caracteres")]
        public string Titulo { get; set; } = string.Empty;



        [StringLength(2000, ErrorMessage = "Sinopse deve ter no máximo 2000 caracteres")]
        public string? Sinopse { get; set; }

        [Range(0, 2024, ErrorMessage = "Ano de publicação inválido")]
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
    }

    public class LivroUpdateDTO : LivroCreateDTO
    {
        public int Id { get; set; }
    }
}
