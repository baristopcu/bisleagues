using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services.Repositories
{
    public class PointTableRowRepository : Repository<PointTableRow>, IPointTableRowRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public PointTableRowRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public IEnumerable<PointTableRow> GetPointTableRowsBySeasonId(int seasonId)
        {
            var pointTableRows = _dbContext.PointTableRows.Where(x => x.SeasonId == seasonId).OrderByDescending(x=>x.Point);
            return pointTableRows;
        }
    }
}
