using System.ComponentModel.DataAnnotations;

namespace BibliotecaUniversitaria.Application.DTOs
{
    public class AutorDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Biografia { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Nacionalidade { get; set; }
    }

    public class AutorCreateDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Biografia deve ter no máximo 2000 caracteres")]
        public string? Biografia { get; set; }

        public DateTime? DataNascimento { get; set; }

        [StringLength(100, ErrorMessage = "Nacionalidade deve ter no máximo 100 caracteres")]
        public string? Nacionalidade { get; set; }
    }

    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }

    public class CategoriaCreateDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }
    }
}
