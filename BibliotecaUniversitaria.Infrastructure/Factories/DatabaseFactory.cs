using Microsoft.EntityFrameworkCore;
using BibliotecaUniversitaria.Infrastructure.Data;

namespace BibliotecaUniversitaria.Infrastructure.Factories
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private ApplicationDbContext? _context;

        public DatabaseFactory(DbContextOptions<ApplicationDbContext> options)
        {
            _options = options;
        }

        public ApplicationDbContext GetContext()
        {
            return _context ??= new ApplicationDbContext(_options);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

    public interface IDatabaseFactory : IDisposable
    {
        ApplicationDbContext GetContext();
    }
}
