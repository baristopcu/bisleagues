

using BisLeagues.Core.Data;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BisLeagues.Core.Services.Repositories
{
    public class ScoreRepository : Repository<Score>, IScoreRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public ScoreRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public IEnumerable<Score> GetAllScoresBySeasonId(int seasonId)
        {
            var scores = _dbContext.Scores.Where(x => x.Result.Match.SeasonId == seasonId);
            return scores;
        }
    }
}
