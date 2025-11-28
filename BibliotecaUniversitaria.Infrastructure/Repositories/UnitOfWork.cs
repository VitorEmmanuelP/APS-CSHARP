using Microsoft.EntityFrameworkCore;
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

        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await Context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<List<(int Id, int Status)>> QueryEmprestimosByLivroIdAsync(int livroId)
        {
            // Usa DbSet com AsNoTracking para evitar rastreamento do EF Core
            var emprestimos = await Context.Set<Domain.Entities.Emprestimo>()
                .Where(e => e.LivroId == livroId)
                .Select(e => new { e.Id, Status = (int)e.Status })
                .AsNoTracking()
                .ToListAsync();

            return emprestimos.Select(e => (e.Id, e.Status)).ToList();
        }

        public async Task<bool> ExistsLivroAsync(int livroId)
        {
            // Usa DbSet com AsNoTracking e AnyAsync para verificar existência sem carregar no contexto
            return await Context.Set<Domain.Entities.Livro>()
                .AsNoTracking()
                .AnyAsync(l => l.Id == livroId);
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}