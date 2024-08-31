using PokemonDomain.Interfaces;
using PokemonInfra.Repository.Entities;
using PokemonInfra.Repository.Interfaces;
using System.Text.Json;

namespace PokemonDomain.Services
{
    public class PokemonService : IPokemonService 
    {
        private readonly HttpClient _httpClient;
        private readonly IPokemonsRepository _pokemonRepository;
        public PokemonService(HttpClient httpClient, IPokemonsRepository pokemonRepository)
        {
            _httpClient = httpClient;
            _pokemonRepository = pokemonRepository;
        }

        public async Task<List<Pokemon>> GetRandomPokemons(int count = 10)
        {
            var randomPokemons = new List<Pokemon>();
            var random = new Random();
            var selectedIds = new HashSet<int>();

            while (randomPokemons.Count < count)
            {
                int pokemonId = random.Next(1, 899);

                if (selectedIds.Add(pokemonId))
                {
                    var pokemon = await GetPokemonById(pokemonId);
                    randomPokemons.Add(pokemon);
                }
            }

            return randomPokemons;
        }

        public async Task<Pokemon> GetPokemonsApiById(int id)
        {
            var response = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Pokemon>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public async Task<Pokemon> GetPokemonById(int id)
        {
            var pokemon = await _pokemonRepository.GetByIdAsync(id);

            if (pokemon == null)
            {
                return null;
            }

            return pokemon;
        }

        public async Task<List<string>> GetCapturedPokemonNamesAsync()
        {
            return await _pokemonRepository.GetCapturedPokemonNamesAsync();
        }

    }
}
