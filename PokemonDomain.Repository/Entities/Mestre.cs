namespace PokemonInfra.Repository.Entities
{
    public class Mestre
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CPF { get; set; }
        public ICollection<MestrePokemon> MestrePokemons { get; set; } = new List<MestrePokemon>();
    }
}