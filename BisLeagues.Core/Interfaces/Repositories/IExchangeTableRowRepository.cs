using System.Collections.Generic;
using BisLeagues.Core.Models;

namespace BisLeagues.Core.Interfaces.Repositories
{
    public interface IExchangeTableRowRepository : IRepository<ExchangeTableRow>
    {
        IEnumerable<ExchangeTableRow> GetExchangeTableRowsBySeasonId(int seasonId);
        IEnumerable<ExchangeTableRow> GetExchangeTableRowsBySeasonId(int seasonId, int skip, int take, out int totalCount);

    }
}
