using PokemonDomain.Models;
using PokemonDomain.Models.Response;
using PokemonInfra.Repository.Entities;

namespace PokemonDomain.Interfaces
{
    public interface IMestreService
    {
        Task<MestreRequest> CreateMestreAsync(MestreRequest mestre);
        Task<Mestre> GetMestreByIdAsync(int id);
        Task<bool> VincularMestreAoPokemonAsync(string cpf, string pokemonName);
        Task<IEnumerable<MestreResponse>> GetAllMestresWithPokemonsAsync();
        Task<MestreResponse> GetPokemonsByMestreIdAsync(int mestreId);

    }
}
