using Microsoft.AspNetCore.Mvc;
using PokemonDomain.Interfaces;
using PokemonDomain.Models;
using PokemonDomain.Models.Response;
using PokemonInfra.Repository.Entities;

namespace PokemonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MestreController : ControllerBase
    {
        private readonly IMestreService _mestreService;

        public MestreController(IMestreService mestreService)
        {
            _mestreService = mestreService;
        }

        [HttpPost]
        public async Task<ActionResult<MestreRequest>> CreateMestre([FromBody] MestreRequest mestre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdMestre = await _mestreService.CreateMestreAsync(mestre);
                return CreatedAtAction(nameof(GetMestreById), new { id = createdMestre.Id }, createdMestre);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mestre>> GetMestreById(int id)
        {
            var mestre = await _mestreService.GetMestreByIdAsync(id);
            if (mestre == null)
            {
                return NotFound();
            }

            return Ok(mestre);
        }

        [HttpPost("vincular")]
        public async Task<IActionResult> VincularMestreAoPokemon([FromBody] VincularRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.CPF) || string.IsNullOrEmpty(request.PokemonName))
            {
                return BadRequest("CPF e nome do Pokémon são obrigatórios.");
            }

            var result = await _mestreService.VincularMestreAoPokemonAsync(request.CPF, request.PokemonName);

            if (!result)
            {
                return NotFound("Mestre ou Pokémon não encontrado.");
            }

            return Ok("Mestre vinculado ao Pokémon com sucesso.");
        }

        [HttpGet("com-pokemons")]
        public async Task<ActionResult<IEnumerable<MestreResponse>>> GetAllMestresWithPokemons()
        {
            var mestres = await _mestreService.GetAllMestresWithPokemonsAsync();
            return Ok(mestres);
        }

        [HttpGet("{id}/pokemons")]
        public async Task<ActionResult<MestreResponse>> GetPokemonsByMestreId(int id)
        {
            var pokemons = await _mestreService.GetPokemonsByMestreIdAsync(id);
            if (pokemons == null)
            {
                return NotFound("Mestre não encontrado ou sem pokémons.");
            }

            return Ok(pokemons);
        }
    }
}
