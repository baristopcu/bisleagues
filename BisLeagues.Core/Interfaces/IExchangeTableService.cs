using System.Collections.Generic;
using System.Threading.Tasks;

namespace BisLeagues.Core.Interfaces
{
    public interface IExchangeTableService
    {
        Task<bool> CreateOrUpdateExchangeTablesForActiveSeasons();
    }
}
