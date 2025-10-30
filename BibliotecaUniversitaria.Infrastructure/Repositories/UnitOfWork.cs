using Microsoft.EntityFrameworkCore.Storage;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Infrastructure.Data;
using BibliotecaUniversitaria.Infrastructure.Repositories;
using BibliotecaUniversitaria.Infrastructure.Factories;

namespace BibliotecaUniversitaria.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        private ApplicationDbContext Context => _context ??= _databaseFactory.GetContext();

        public IAutorRepository Autores => new AutorRepository(Context);
        public ICategoriaRepository Categorias => new CategoriaRepository(Context);
        public ILivroRepository Livros => new LivroRepository(Context);
        public IEmprestimoRepository Emprestimos => new EmprestimoRepository(Context);
        public IMultaRepository Multas => new MultaRepository(Context);

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await Context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}