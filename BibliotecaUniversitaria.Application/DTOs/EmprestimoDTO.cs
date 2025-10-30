using System.ComponentModel.DataAnnotations;

namespace BibliotecaUniversitaria.Application.DTOs
{
    public class EmprestimoDTO
    {
        public int Id { get; set; }
        public int LivroId { get; set; }
        public string LivroTitulo { get; set; } = string.Empty;
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucaoPrevista { get; set; }
        public DateTime? DataDevolucaoReal { get; set; }
        public int QuantidadeEmprestada { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
    }

    public class EmprestimoCreateDTO
    {
        [Required(ErrorMessage = "Livro é obrigatório")]
        public int LivroId { get; set; }

        [Required(ErrorMessage = "Data de empréstimo é obrigatória")]
        public DateTime DataEmprestimo { get; set; }

        [Required(ErrorMessage = "Data de devolução prevista é obrigatória")]
        public DateTime DataDevolucaoPrevista { get; set; }

        [Required(ErrorMessage = "Quantidade emprestada é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
        public int QuantidadeEmprestada { get; set; }

        [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }
    }

    public class EmprestimoDevolucaoDTO
    {
        [Required(ErrorMessage = "Data de devolução é obrigatória")]
        public DateTime DataDevolucaoReal { get; set; }

        [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }
    }
}
