using PokemonInfra.Repository.Entities;

namespace PokemonDomain.Interfaces
{
    public interface  IPokemonService
    {
        Task<List<Pokemon>> GetRandomPokemons(int count = 10);
        Task<Pokemon> GetPokemonsApiById(int id);
        Task<Pokemon> GetPokemonById(int id);
        Task<List<string>> GetCapturedPokemonNamesAsync();
    }
}
