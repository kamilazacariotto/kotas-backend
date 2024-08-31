using FluentAssertions;
using Moq;
using PokemonDomain.Services;
using PokemonInfra.Repository.Entities;
using PokemonInfra.Repository.Interfaces;

namespace PokemonUnitTest
{
    [TestClass]
    public class PokemonServiceTests
    {

        private Mock<IPokemonsRepository> _pokemonRepositoryMock;
        private PokemonService _pokemonService;

        [TestInitialize]
        public void Setup()
        {
            _pokemonRepositoryMock = new Mock<IPokemonsRepository>();

            var httpClient = new HttpClient();

            _pokemonService = new PokemonService(httpClient, _pokemonRepositoryMock.Object);
        }

        [TestMethod]
        public async Task GetCapturedPokemonNamesAsync_ShouldReturnEmptyList_WhenNoPokemonsCaptured()
        {
            _pokemonRepositoryMock.Setup(repo => repo.GetCapturedPokemonNamesAsync())
                                  .ReturnsAsync(new List<string>());

            // Act
            var result = await _pokemonService.GetCapturedPokemonNamesAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public async Task GetCapturedPokemonNamesAsync_ShouldReturnPokemonNames_WhenPokemonsCaptured()
        {
            // Arrange
            var capturedPokemons = new List<string> { "Pikachu", "Charmander" };
            _pokemonRepositoryMock.Setup(repo => repo.GetCapturedPokemonNamesAsync())
                                  .ReturnsAsync(capturedPokemons);

            // Act
            var result = await _pokemonService.GetCapturedPokemonNamesAsync();

            // Assert: Verificação se o resultado corresponde à lista de nomes de Pokémon
            result.Should().BeEquivalentTo(capturedPokemons);
        }

        [TestMethod]
        public async Task GetCapturedPokemonNamesAsync_ShouldReturnSinglePokemon_WhenOnlyOnePokemonCaptured()
        {
            // Arrange
            var capturedPokemons = new List<string> { "Bulbasaur" };
            _pokemonRepositoryMock.Setup(repo => repo.GetCapturedPokemonNamesAsync())
                                  .ReturnsAsync(capturedPokemons);

            // Act
            var result = await _pokemonService.GetCapturedPokemonNamesAsync();

            // Assert
            result.Should().ContainSingle()
                  .Which.Should().Be("Bulbasaur");
        }

        [TestMethod]
        public async Task GetCapturedPokemonNamesAsync_ShouldReturnOrderedPokemonNames_WhenMultiplePokemonsCaptured()
        {
            // Arrange
            var capturedPokemons = new List<string> { "Charmander", "Bulbasaur", "Squirtle" };
            _pokemonRepositoryMock.Setup(repo => repo.GetCapturedPokemonNamesAsync())
                                  .ReturnsAsync(capturedPokemons);

            // Act
            var result = await _pokemonService.GetCapturedPokemonNamesAsync();

            // Assert
            result.Should().ContainInOrder("Charmander", "Bulbasaur", "Squirtle");
        }

        [TestMethod]
        public async Task GetPokemonById_ShouldReturnNull_WhenPokemonNotFound()
        {
            // Arrange
            _pokemonRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync((Pokemon)null);

            // Act
            var result = await _pokemonService.GetPokemonById(1);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task GetPokemonById_ShouldReturnPokemon_WhenPokemonIsFound()
        {
            // Arrange
            var expectedPokemon = new Pokemon { Id = 1, Name = "Pikachu" };
            _pokemonRepositoryMock.Setup(repo => repo.GetByIdAsync(expectedPokemon.Id))
                                  .ReturnsAsync(expectedPokemon);

            // Act
            var result = await _pokemonService.GetPokemonById(expectedPokemon.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedPokemon);
        }
    }
}
