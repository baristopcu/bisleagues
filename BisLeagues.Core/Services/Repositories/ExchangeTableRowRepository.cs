using BisLeagues.Core.Data;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BisLeagues.Core.Services.Repositories
{
    public class ExchangeTableRowRepository : Repository<ExchangeTableRow>, IExchangeTableRowRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public ExchangeTableRowRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public IEnumerable<ExchangeTableRow> GetExchangeTableRowsBySeasonId(int seasonId)
        {
            var exchangeTableRows = _dbContext.ExchangeTableRows.Where(x => x.SeasonId == seasonId).OrderByDescending(x=>x.Value);
            return exchangeTableRows;
        }
        public IEnumerable<ExchangeTableRow> GetExchangeTableRowsBySeasonId(int seasonId, int skip, int take, out int totalCount)
        {
            totalCount = _dbContext.ExchangeTableRows.Where(x => x.SeasonId == seasonId).Count();
            var exchangeTableRows = _dbContext.ExchangeTableRows.Where(x => x.SeasonId == seasonId).OrderByDescending(x => x.Value).Skip(skip).Take(take);
            return exchangeTableRows;
        }
    }
}
