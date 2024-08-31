using Microsoft.AspNetCore.Mvc;
using PokemonDomain.Interfaces;
using PokemonInfra.Repository.Entities;

namespace PokemonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pokemon>> GetPokemon(int id)
        {
            var pokemon = await _pokemonService.GetPokemonById(id);
            if (pokemon == null)
            {
                return NotFound();
            }
            return Ok(pokemon);
        }

        [HttpGet("random")]
        public async Task<ActionResult<IEnumerable<Pokemon>>> GetRandomPokemons()
        {
            var pokemons = await _pokemonService.GetRandomPokemons(2);
            return Ok(pokemons);
        }

        [HttpGet("capturados")]
        public async Task<ActionResult<List<string>>> GetCapturedPokemonNames()
        {
            var pokemonNames = await _pokemonService.GetCapturedPokemonNamesAsync();
            return Ok(pokemonNames);
        }
    }
}