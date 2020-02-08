using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services.Repositories
{
    public class MatchRepository : Repository<Match>, IMatchRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public MatchRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public IEnumerable<Match> GetMatchesBySeasonId(int seasonId)
        {
            var matches = _dbContext.Matches.Where(x => x.SeasonId == seasonId);
            return matches;
        }

        public Match GetUpcomingMatchBySeasonId(int seasonId)
        {
            DateTime fromTime = DateTime.UtcNow;
            Match match =  _dbContext.Matches.Where(x=> x.SeasonId==seasonId && x.MatchDate > fromTime).OrderBy(m => m.MatchDate).FirstOrDefault();
            return match;
        }

        public IEnumerable<Match> GetUpcomingMatchesBySeasonId(int seasonId)
        {
            IEnumerable<Match> matches = _dbContext.Matches.Where(x => x.SeasonId == seasonId && x.IsPlayed == false && x.MatchDate > DateTime.UtcNow).OrderBy(p => p.MatchDate);
            return matches;
        }

        public IEnumerable<Match> GetUpcomingMatchesBySeasonIdAndLimit(int seasonId, int limit)
        {
            IEnumerable<Match> matches = _dbContext.Matches.Where(x => x.SeasonId == seasonId && x.IsPlayed == false && x.MatchDate > DateTime.UtcNow).OrderBy(p => p.MatchDate).Take(limit);
            return matches;
        }
    }
}
