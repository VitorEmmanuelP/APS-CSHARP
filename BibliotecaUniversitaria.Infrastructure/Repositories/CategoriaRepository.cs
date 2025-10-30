using Microsoft.EntityFrameworkCore;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Infrastructure.Data;

namespace BibliotecaUniversitaria.Infrastructure.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Categoria?> GetByNomeAsync(string nome)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Nome == nome);
        }
    }
}
