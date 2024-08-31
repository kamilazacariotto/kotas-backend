namespace PokemonInfra.Repository.Entities
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? AdditionalInfo { get; set; }
        public ICollection<MestrePokemon> MestrePokemons { get; set; } = new List<MestrePokemon>();
    }
}
