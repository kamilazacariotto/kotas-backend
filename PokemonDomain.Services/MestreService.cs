using PokemonDomain.Interfaces;
using PokemonDomain.Models;
using PokemonDomain.Models.Response;
using PokemonInfra.Repository.Entities;
using PokemonInfra.Repository.Interfaces;

namespace PokemonDomain.Services
{
    public class MestreService : IMestreService
    {
        private readonly IMestresRepository _mestreRepository;
        private readonly IPokemonsRepository _pokemonRepository;
        private readonly IMestrePokemonRepository _mestrePokemonRepository;

        public MestreService(IMestresRepository mestreRepository, IPokemonsRepository pokemonRepository, IMestrePokemonRepository mestrePokemonRepository)
        {
            _mestreRepository = mestreRepository;
            _pokemonRepository = pokemonRepository;
            _mestrePokemonRepository = mestrePokemonRepository;
        }

        public async Task<MestreRequest> CreateMestreAsync(MestreRequest mestreRequest)
        {
            Mestre mestre = new Mestre();
            if (mestreRequest == null)
            {
                throw new ArgumentNullException(nameof(mestre));
            }

            if (mestreRequest.Name.Length < 3 || mestreRequest.Name.Length > 100)
            {
                throw new ArgumentException("O nome do mestre deve ter entre 3 e 100 caracteres.");
            }

            if (!IsValidCPF(mestreRequest.CPF))
            {
                throw new ArgumentException("O CPF informado não é válido.");
            }

            var existingMestre = await _mestreRepository
                .GetbyCPF(mestreRequest.CPF);

            if (existingMestre != null)
            {
                throw new ArgumentException("Já existe um mestre cadastrado com este CPF.");
            }

            mestre.Name = mestreRequest.Name;
            mestre.CPF = mestreRequest.CPF;

            await _mestreRepository.AddAsync(mestre);

            mestreRequest.Id = mestre.Id;
            return mestreRequest;
        }
        public async Task<Mestre> GetMestreByIdAsync(int id)
        {
            return await _mestreRepository.GetByIdAsync(id);
        }

        public async Task<bool> VincularMestreAoPokemonAsync(string cpf, string pokemonName)
        {
            MestrePokemon mestrePokemon = new MestrePokemon();
            var mestre = (await _mestreRepository.GetAllAsync())
                         .FirstOrDefault(m => m.CPF == cpf);

            if (mestre == null)
            {
                return false; 
            }

            var pokemon = (await _pokemonRepository.GetAllAsync())
                          .FirstOrDefault(p => p.Name == pokemonName);

            if (pokemon == null)
            {
                return false;
            }

            mestrePokemon.MestreId = mestre.Id;
            mestrePokemon.PokemonId = pokemon.Id;

            await _mestrePokemonRepository.AddAsync(mestrePokemon);

            return true;
        }
        public async Task<IEnumerable<MestreResponse>> GetAllMestresWithPokemonsAsync()
        {
            var mestres = await _mestreRepository.GetAllMestresWithPokemonsAsync();
            var retorno = mestres.Select(mestre => new MestreResponse
            {
                Name = mestre.Name,
                CPF = mestre.CPF,
                Pokemons = mestre.MestrePokemons.Select(mp => new PokemonResponse
                {
                    Name = mp.Pokemon.Name,
                    AdditionalInfo = mp.Pokemon.AdditionalInfo
                }).ToList()
            });
            return retorno;
        }
        public async Task<MestreResponse> GetPokemonsByMestreIdAsync(int mestreId)
        {
            var mestre = await _mestreRepository.GetByIdAsync(mestreId);
            if (mestre == null)
            {
                return null;
            }

            var mestreResponse = new MestreResponse
            {
                Name = mestre.Name,
                CPF = mestre.CPF,
                Pokemons = mestre.MestrePokemons.Select(mp => new PokemonResponse
                {
                    Name = mp.Pokemon.Name,
                    AdditionalInfo = mp.Pokemon.AdditionalInfo
                }).ToList()
            };

            return mestreResponse;
        }
        private bool IsValidCPF(string cpf)
        {
            if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            {
                return false;
            }

            return true;
        }
    }
}
