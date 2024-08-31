using PokemonInfra.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PokemonInfra.Repository.Interfaces
{
    public interface IMestresRepository : IRepository<Mestre>
    {
        Task<IEnumerable<Mestre>> GetAllMestresWithPokemonsAsync();
        Task<Mestre> GetbyCPF(string cpf);
    }
}
