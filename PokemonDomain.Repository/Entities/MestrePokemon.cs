namespace PokemonInfra.Repository.Entities
{
    public class MestrePokemon
    {
        public int Id { get; set; }
        public int MestreId { get; set; } 
        public int PokemonId { get; set; } 
        public Pokemon Pokemon { get; set; }
        public Mestre Mestre { get; set; } 
    }
}
