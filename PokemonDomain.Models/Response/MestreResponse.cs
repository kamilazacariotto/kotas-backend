using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonDomain.Models.Response
{
    public class MestreResponse
    {
        public string? Name { get; set; }
        public string? CPF { get; set; }
        public List<PokemonResponse>? Pokemons { get; set; }
    }
}
