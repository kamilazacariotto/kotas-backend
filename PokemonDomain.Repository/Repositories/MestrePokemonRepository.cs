using PokemonInfra.Repository.Entities;
using PokemonInfra.Repository.Interfaces;

namespace PokemonInfra.Repository.Repositories
{
    public class MestrePokemonRepository : Repository<MestrePokemon>, IMestrePokemonRepository
    {
        public MestrePokemonRepository(PokemonDbContext context) : base(context)
        {
        }
    }
}
