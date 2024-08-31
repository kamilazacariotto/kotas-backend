using System.ComponentModel.DataAnnotations;

namespace PokemonDomain.Models
{
    public class MestreRequest
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string CPF { get; set; }
    }
}
