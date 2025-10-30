using BibliotecaUniversitaria.Domain.Entities;

namespace BibliotecaUniversitaria.Application.Interfaces
{
    public interface IAutorRepository : IRepository<Autor>
    {
        Task<Autor?> GetByNomeAsync(string nome);
        Task<IEnumerable<Autor>> GetByNacionalidadeAsync(string nacionalidade);
    }

    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<Categoria?> GetByNomeAsync(string nome);
    }

    public interface ILivroRepository : IRepository<Livro>
    {
        Task<IEnumerable<Livro>> GetByTituloAsync(string titulo);
        Task<IEnumerable<Livro>> GetByAutorIdAsync(int autorId);
        Task<IEnumerable<Livro>> GetByCategoriaIdAsync(int categoriaId);
        Task<IEnumerable<Livro>> GetDisponiveisAsync();
    }

    public interface IEmprestimoRepository : IRepository<Emprestimo>
    {
        Task<IEnumerable<Emprestimo>> GetByLivroIdAsync(int livroId);
        Task<IEnumerable<Emprestimo>> GetAtivosAsync();
        Task<IEnumerable<Emprestimo>> GetAtrasadosAsync();
        Task<IEnumerable<Emprestimo>> GetByStatusAsync(Domain.Enums.StatusEmprestimo status);
    }

    public interface IMultaRepository : IRepository<Multa>
    {
        Task<IEnumerable<Multa>> GetByEmprestimoIdAsync(int emprestimoId);
        Task<IEnumerable<Multa>> GetPendentesAsync();
        Task<IEnumerable<Multa>> GetVencidasAsync();
        Task<IEnumerable<Multa>> GetByStatusAsync(Domain.Enums.StatusMulta status);
    }
}
