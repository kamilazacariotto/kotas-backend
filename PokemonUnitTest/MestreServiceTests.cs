using FluentAssertions;
using Moq;
using PokemonDomain.Models;
using PokemonDomain.Services;
using PokemonInfra.Repository.Entities;
using PokemonInfra.Repository.Interfaces;

namespace PokemonUnitTest
{
    [TestClass]
    public class MestreServiceTests
    {
        private Mock<IMestresRepository> _mestreRepositoryMock;
        private Mock<IPokemonsRepository> _pokemonRepositoryMock;
        private Mock<IMestrePokemonRepository> _mestrePokemonRepositoryMock;
        private MestreService _mestreService;

        [TestInitialize]
        public void Setup()
        {
            _mestreRepositoryMock = new Mock<IMestresRepository>();
            _pokemonRepositoryMock = new Mock<IPokemonsRepository>();
            _mestrePokemonRepositoryMock = new Mock<IMestrePokemonRepository>();

            _mestreService = new MestreService(
                _mestreRepositoryMock.Object,
                _pokemonRepositoryMock.Object,
                _mestrePokemonRepositoryMock.Object
            );
        }
        [TestMethod]
        public async Task CreateMestreAsync_ShouldThrowArgumentNullException_WhenMestreIsNull()
        {
            // Act
            Func<Task> action = async () => await _mestreService.CreateMestreAsync(null);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                        .WithMessage("Value cannot be null. (Parameter 'mestre')");
        }

        [TestMethod]
        public async Task CreateMestreAsync_ShouldThrowArgumentException_WhenNameIsTooShort()
        {
            // Arrange
            var mestre = new MestreRequest { Name = "As", CPF = "12345678901" };

            // Act
            Func<Task> action = async () => await _mestreService.CreateMestreAsync(mestre);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>()
                        .WithMessage("O nome do mestre deve ter entre 3 e 100 caracteres.");
        }

        [TestMethod]
        public async Task CreateMestreAsync_ShouldThrowArgumentException_WhenNameIsTooLong()
        {
            // Arrange
            var mestre = new MestreRequest { Name = new string('A', 101), CPF = "12345678901" };

            // Act
            Func<Task> action = async () => await _mestreService.CreateMestreAsync(mestre);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>()
                        .WithMessage("O nome do mestre deve ter entre 3 e 100 caracteres.");
        }

        [TestMethod]
        public async Task CreateMestreAsync_ShouldThrowArgumentException_WhenCPFIsInvalid()
        {
            // Arrange
            var mestre = new MestreRequest { Name = "Ash Ketchum", CPF = "12345" };

            // Act
            Func<Task> action = async () => await _mestreService.CreateMestreAsync(mestre);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>()
                        .WithMessage("O CPF informado não é válido.");
        }

        //[TestMethod]
        //public async Task CreateMestreAsync_ShouldThrowArgumentException_WhenCPFAlreadyExists()
        //{
        //    // Arrange
        //    var mestre = new MestreRequest { Name = "Ash Ketchum", CPF = "12345678901" };
        //    _mestreRepositoryMock.Setup(repo => repo.GetbyCPF(mestre.CPF))
        //                        .ReturnsAsync(mestre);

        //    // Act
        //    Func<Task> action = async () => await _mestreService.CreateMestreAsync(mestre);

        //    // Assert
        //    await action.Should().ThrowAsync<ArgumentException>()
        //                .WithMessage("Já existe um mestre cadastrado com este CPF.");
        //}

        [TestMethod]
        public async Task CreateMestreAsync_ShouldReturnMestre_WhenMestreIsValid()
        {
            // Arrange
            var mestreRequest = new MestreRequest { Name = "Ash Ketchum", CPF = "12345678901" };
            _mestreRepositoryMock.Setup(repo => repo.GetByCPF(mestreRequest.CPF))
                                 .ReturnsAsync((Mestre)null);

            var mestre = new Mestre { Name = mestreRequest.Name, CPF = mestreRequest.CPF };
            _mestreRepositoryMock.Setup(repo => repo.AddAsync(mestre))
                                 .Returns(Task.CompletedTask);

            // Act
            var result = await _mestreService.CreateMestreAsync(mestreRequest);

            // Assert
            result.Should().BeEquivalentTo(mestreRequest);
        }


        [TestMethod]
        public async Task VincularMestreAoPokemonAsync_ShouldReturnFalse_WhenMestreNotFound()
        {
            // Arrange
            _mestreRepositoryMock.Setup(repo => repo.GetAllAsync())
                                 .ReturnsAsync(new List<Mestre>());

            // Act
            var result = await _mestreService.VincularMestreAoPokemonAsync("12345678901", "Pikachu");

            // Assert
            result.Should().BeFalse();
            _mestrePokemonRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<MestrePokemon>()), Times.Never);
        }

        [TestMethod]
        public async Task VincularMestreAoPokemonAsync_ShouldReturnFalse_WhenPokemonNotFound()
        {
            // Arrange
            var mestre = new Mestre { Id = 1, CPF = "12345678901" };
            _mestreRepositoryMock.Setup(repo => repo.GetAllAsync())
                                 .ReturnsAsync(new List<Mestre> { mestre });

            _pokemonRepositoryMock.Setup(repo => repo.GetAllAsync())
                                  .ReturnsAsync(new List<Pokemon>());

            // Act
            var result = await _mestreService.VincularMestreAoPokemonAsync("12345678901", "Pikachu");

            // Assert
            result.Should().BeFalse();
            _mestrePokemonRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<MestrePokemon>()), Times.Never);
        }

        [TestMethod]
        public async Task VincularMestreAoPokemonAsync_ShouldReturnTrue_WhenMestreAndPokemonAreFound()
        {
            // Arrange
            var mestre = new Mestre { Id = 1, CPF = "12345678901" };
            var pokemon = new Pokemon { Id = 1, Name = "Pikachu" };
            _mestreRepositoryMock.Setup(repo => repo.GetAllAsync())
                                 .ReturnsAsync(new List<Mestre> { mestre });

            _pokemonRepositoryMock.Setup(repo => repo.GetAllAsync())
                                  .ReturnsAsync(new List<Pokemon> { pokemon });

            // Act
            var result = await _mestreService.VincularMestreAoPokemonAsync("12345678901", "Pikachu");

            // Assert
            result.Should().BeTrue();
            _mestrePokemonRepositoryMock.Verify(repo => repo.AddAsync(It.Is<MestrePokemon>(mp =>
                mp.MestreId == mestre.Id && mp.PokemonId == pokemon.Id
            )), Times.Once);
        }

        [TestMethod]
        public async Task VincularMestreAoPokemonAsync_ShouldSaveMestrePokemon_WhenMestreAndPokemonAreFound()
        {
            // Arrange
            var mestre = new Mestre { Id = 1, CPF = "12345678901" };
            var pokemon = new Pokemon { Id = 1, Name = "Pikachu" };
            _mestreRepositoryMock.Setup(repo => repo.GetAllAsync())
                                 .ReturnsAsync(new List<Mestre> { mestre });

            _pokemonRepositoryMock.Setup(repo => repo.GetAllAsync())
                                  .ReturnsAsync(new List<Pokemon> { pokemon });

            // Act
            var result = await _mestreService.VincularMestreAoPokemonAsync("12345678901", "Pikachu");

            // Assert
            result.Should().BeTrue();
            _mestrePokemonRepositoryMock.Verify(repo => repo.AddAsync(It.Is<MestrePokemon>(mp =>
                mp.MestreId == mestre.Id && mp.PokemonId == pokemon.Id
            )), Times.Once);
        }
        [TestMethod]
        public async Task GetPokemonsByMestreIdAsync_ShouldReturnNull_WhenMestreNotFound()
        {
            // Arrange
            _mestreRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                 .ReturnsAsync((Mestre)null);

            // Act
            var result = await _mestreService.GetPokemonsByMestreIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task GetPokemonsByMestreIdAsync_ShouldReturnMestreWithPokemons_WhenMestreIsFound()
        {
            // Arrange
            var mestre = new Mestre
            {
                Id = 1,
                Name = "Ash Ketchum",
                CPF = "12345678901",
                MestrePokemons = new List<MestrePokemon>
                {
                    new MestrePokemon
                    {
                        Pokemon = new Pokemon
                        {
                            Name = "Pikachu",
                            AdditionalInfo = "Electric Type"
                        }
                    }
                }
            };

            _mestreRepositoryMock.Setup(repo => repo.GetByIdAsync(mestre.Id))
                                 .ReturnsAsync(mestre);

            // Act
            var result = await _mestreService.GetPokemonsByMestreIdAsync(mestre.Id);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Ash Ketchum");
            result.CPF.Should().Be("12345678901");
            result.Pokemons.Should().ContainSingle();
            result.Pokemons.First().Name.Should().Be("Pikachu");
            result.Pokemons.First().AdditionalInfo.Should().Be("Electric Type");
        }

        [TestMethod]
        public async Task GetAllMestresWithPokemonsAsync_ShouldReturnEmptyList_WhenNoMestresFound()
        {
            // Arrange
            _mestreRepositoryMock.Setup(repo => repo.GetAllMestresWithPokemonsAsync())
                                 .ReturnsAsync(new List<Mestre>());

            // Act
            var result = await _mestreService.GetAllMestresWithPokemonsAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public async Task GetAllMestresWithPokemonsAsync_ShouldReturnMestresWithPokemons_WhenMestresAreFound()
        {
            // Arrange
            var mestres = new List<Mestre>
            {
                new Mestre
                {
                    Name = "Ash Ketchum",
                    CPF = "12345678901",
                    MestrePokemons = new List<MestrePokemon>
                    {
                        new MestrePokemon
                        {
                            Pokemon = new Pokemon
                            {
                                Name = "Pikachu",
                                AdditionalInfo = "Electric Type"
                            }
                        }
                    }
                }
            };

            _mestreRepositoryMock.Setup(repo => repo.GetAllMestresWithPokemonsAsync())
                                 .ReturnsAsync(mestres);

            // Act
            var result = await _mestreService.GetAllMestresWithPokemonsAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.First().Name.Should().Be("Ash Ketchum");
            result.First().CPF.Should().Be("12345678901");
            result.First().Pokemons.Should().ContainSingle();
            result.First().Pokemons.First().Name.Should().Be("Pikachu");
            result.First().Pokemons.First().AdditionalInfo.Should().Be("Electric Type");
        }

    }
}
