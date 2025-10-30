using Microsoft.EntityFrameworkCore;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Domain.Entities;
using BibliotecaUniversitaria.Domain.Enums;
using BibliotecaUniversitaria.Infrastructure.Data;

namespace BibliotecaUniversitaria.Infrastructure.Repositories
{
    public class MultaRepository : Repository<Multa>, IMultaRepository
    {
        public MultaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Multa>> GetByEmprestimoIdAsync(int emprestimoId)
        {
            return await _dbSet.Where(m => m.EmprestimoId == emprestimoId).ToListAsync();
        }

        public async Task<IEnumerable<Multa>> GetPendentesAsync()
        {
            return await _dbSet.Where(m => m.Status == StatusMulta.Pendente).ToListAsync();
        }

        public async Task<IEnumerable<Multa>> GetVencidasAsync()
        {
            var hoje = DateTime.Now;
            return await _dbSet.Where(m => m.Status == StatusMulta.Pendente && m.DataVencimento < hoje).ToListAsync();
        }

        public async Task<IEnumerable<Multa>> GetByStatusAsync(StatusMulta status)
        {
            return await _dbSet.Where(m => m.Status == status).ToListAsync();
        }
    }
}
