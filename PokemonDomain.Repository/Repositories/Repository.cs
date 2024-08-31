using Microsoft.EntityFrameworkCore;
using PokemonInfra.Repository.Interfaces;
using System.Linq.Expressions;

namespace PokemonInfra.Repository.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly PokemonDbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(PokemonDbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
            _context.SaveChanges();
        }
    }
}
