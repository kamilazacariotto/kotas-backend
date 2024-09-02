using System.ComponentModel.DataAnnotations;

namespace PokemonDomain.Models
{
    public class VincularRequest
    {
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string? CPF { get; set; }

        [Required(ErrorMessage = "O Pokemon é obrigatório.")]
        public string? PokemonName { get; set; }
    }
}