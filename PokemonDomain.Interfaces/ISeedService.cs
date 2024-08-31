using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonDomain.Interfaces
{
    public interface ISeedService
    {
        Task SeedPokemonsAsync();
    }
}
