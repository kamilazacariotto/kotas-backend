using PokemonInfra.Repository.Entities;

namespace PokemonInfra.Repository.Interfaces
{
    public interface IPokemonsRepository : IRepository<Pokemon>
    {
        Task<List<string>> GetCapturedPokemonNamesAsync();
    }

}
