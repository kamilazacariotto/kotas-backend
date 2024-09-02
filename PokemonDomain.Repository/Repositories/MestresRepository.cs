using Microsoft.EntityFrameworkCore;
using PokemonInfra.Repository.Entities;
using PokemonInfra.Repository.Interfaces;
using System.Linq.Expressions;

namespace PokemonInfra.Repository.Repositories
{
    public class MestresRepository : Repository<Mestre>, IMestresRepository
    {
        public MestresRepository(PokemonDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Mestre>> GetAllWithIncludesAsync(params Expression<Func<Mestre, object>>[] includes)
        {
            IQueryable<Mestre> query = _context.Mestres;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }
        
        public async Task<Mestre> GetByIdAsync(int id)
        {
            return await _context.Mestres
                .Include(m => m.MestrePokemons)
                .ThenInclude(mp => mp.Pokemon)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Mestre>> GetAllMestresWithPokemonsAsync()
        {
            return await _context.Mestres
                .Include(m => m.MestrePokemons)
                .ThenInclude(mp => mp.Pokemon)
                .ToListAsync();
        }
        public async Task<Mestre> GetByCPF(string cpf)
        {
            return await _context.Mestres
                     .Where(x => x.CPF.Equals(cpf))
                     .FirstOrDefaultAsync();
        }
    }
}
