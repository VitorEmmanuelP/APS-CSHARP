using Microsoft.EntityFrameworkCore;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Domain.Enums;
using BibliotecaUniversitaria.Infrastructure.Data;

namespace BibliotecaUniversitaria.Infrastructure.Repositories
{
    public class EmprestimoRepository : Repository<Emprestimo>, IEmprestimoRepository
    {
        public EmprestimoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Emprestimo>> GetAllAsync()
        {
            return await _dbSet
                .Include(e => e.Livro)
                .ToListAsync();
        }

        public override async Task<Emprestimo?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(e => e.Livro)
                .FirstOrDefaultAsync(e => e.Id == id);
        }


        public async Task<IEnumerable<Emprestimo>> GetByLivroIdAsync(int livroId)
        {
            return await _dbSet.Where(e => e.LivroId == livroId).ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> GetAtivosAsync()
        {
            return await _dbSet
                .Include(e => e.Livro)
                .Where(e => e.Status == StatusEmprestimo.Ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> GetAtrasadosAsync()
        {
            var hoje = DateTime.Now;
            return await _dbSet
                .Include(e => e.Livro)
                .Where(e => e.Status == StatusEmprestimo.Ativo && e.DataDevolucaoPrevista < hoje)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> GetByStatusAsync(StatusEmprestimo status)
        {
            return await _dbSet
                .Include(e => e.Livro)
                .Where(e => e.Status == status)
                .ToListAsync();
        }
    }
}
