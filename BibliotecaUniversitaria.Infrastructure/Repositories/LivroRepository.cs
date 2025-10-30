using Microsoft.EntityFrameworkCore;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Infrastructure.Data;

namespace BibliotecaUniversitaria.Infrastructure.Repositories
{
    public class LivroRepository : Repository<Livro>, ILivroRepository
    {
        public LivroRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Livro?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public override async Task<IEnumerable<Livro>> GetAllAsync()
        {
            return await _dbSet
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .ToListAsync();
        }



        public async Task<IEnumerable<Livro>> GetByTituloAsync(string titulo)
        {
            return await _dbSet
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Where(l => l.Titulo.Contains(titulo))
                .ToListAsync();
        }

        public async Task<IEnumerable<Livro>> GetByAutorIdAsync(int autorId)
        {
            return await _dbSet
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Where(l => l.AutorId == autorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Livro>> GetByCategoriaIdAsync(int categoriaId)
        {
            return await _dbSet
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Where(l => l.CategoriaId == categoriaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Livro>> GetDisponiveisAsync()
        {
            return await _dbSet
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Where(l => l.QuantidadeDisponivel > 0)
                .ToListAsync();
        }
    }
}
