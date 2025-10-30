namespace BibliotecaUniversitaria.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAutorRepository Autores { get; }
        ICategoriaRepository Categorias { get; }
        ILivroRepository Livros { get; }
        IEmprestimoRepository Emprestimos { get; }
        IMultaRepository Multas { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
