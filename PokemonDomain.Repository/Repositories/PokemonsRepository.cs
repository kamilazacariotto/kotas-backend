using Microsoft.EntityFrameworkCore;
using PokemonInfra.Repository.Entities;
using PokemonInfra.Repository.Interfaces;

namespace PokemonInfra.Repository.Repositories
{
    public class PokemonsRepository : Repository<Pokemon>, IPokemonsRepository
    {
        public PokemonsRepository(PokemonDbContext context) : base(context)
        {
        }
        public async Task<List<string>> GetCapturedPokemonNamesAsync()
        {
            return await _context.MestrePokemons
                .Include(mp => mp.Pokemon)
                .Select(mp => mp.Pokemon.Name)
                .Distinct()
                .ToListAsync();
        }
    }
}
