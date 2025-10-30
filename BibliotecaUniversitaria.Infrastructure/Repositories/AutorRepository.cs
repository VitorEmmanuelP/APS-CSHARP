using Microsoft.EntityFrameworkCore;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Infrastructure.Data;

namespace BibliotecaUniversitaria.Infrastructure.Repositories
{
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        public AutorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Autor?> GetByNomeAsync(string nome)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.Nome == nome);
        }

        public async Task<IEnumerable<Autor>> GetByNacionalidadeAsync(string nacionalidade)
        {
            return await _dbSet.Where(a => a.Nacionalidade == nacionalidade).ToListAsync();
        }
    }
}
