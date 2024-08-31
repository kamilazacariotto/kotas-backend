using PokemonDomain.Interfaces;
using PokemonInfra.Repository.Entities;
using PokemonInfra.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokemonDomain.Services
{
    public class SeedService : ISeedService
    {
        private readonly PokemonDbContext _context;
        private readonly PokemonService _pokemonService;

        public SeedService(PokemonDbContext context, PokemonService pokemonService)
        {
            _context = context;
            _pokemonService = pokemonService;
        }

        public async Task SeedPokemonsAsync()
        {
            if (!_context.Pokemons.Any())
            {
                var allPokemons = await GetAllPokemonsAsync();

                foreach (var pokemon in allPokemons)
                {
                    var additionalInfo = JsonSerializer.Serialize(pokemon);

                    var newPokemon = new Pokemon
                    {
                        Id = pokemon.Id,
                        Name = pokemon.Name,
                        AdditionalInfo = additionalInfo
                    };

                    _context.Pokemons.Add(newPokemon);
                }

                await _context.SaveChangesAsync();
            }
        }

        private async Task<List<Pokemon>> GetAllPokemonsAsync()
        {
            var pokemons = new List<Pokemon>();

            for (int i = 1; i <= 899; i++) 
            {
                Console.WriteLine("Pokemnon ID: ", i);
                var pokemon = await _pokemonService.GetPokemonsApiById(i);
                pokemons.Add(pokemon);
            }

            return pokemons;
        }
    }
}
