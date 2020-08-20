﻿using BisLeagues.Core.Data;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BisLeagues.Core.Services.Repositories
{
    public class GoalKingRowRepository : Repository<GoalKingRow>, IGoalKingRowRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public GoalKingRowRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public IEnumerable<GoalKingRow> GetGoalKingTableRowsBySeasonId(int seasonId)
        {
            var goalKingRows = _dbContext.GoalKingRows.Where(x => x.SeasonId == seasonId).OrderByDescending(x=>x.Goals);
            return goalKingRows;
        }
    }
}
