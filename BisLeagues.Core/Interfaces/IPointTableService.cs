using System.Collections.Generic;
using System.Threading.Tasks;
using BisLeagues.Core.Models;

namespace BisLeagues.Core.Interfaces
{
    public interface IPointTableService
    {
        Task<bool> CreatePointTablesForActiveSeasons();
    }
}
