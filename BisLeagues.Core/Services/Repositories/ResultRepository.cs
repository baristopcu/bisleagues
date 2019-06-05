using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BisLeagues.Core.Services.Repositories
{
    public class ResultRepository : Repository<Result>, IResultRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public ResultRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Result GetLastMatchsResult()
        {
            DateTime fromTime = DateTime.UtcNow;
            Result result = _dbContext.Results.Where(x => x.Match.MatchDate < fromTime && x.Match.IsPlayed == true).OrderByDescending(m => m.Match.MatchDate).FirstOrDefault();
            return result;
        }

        public IEnumerable<Result> GetResultsOfMatches(IEnumerable<Match> matches)
        {
            var results = _dbContext.Results.Where(x => matches.Select(y=>y.Id).Contains(x.MatchId));
            return results;
        }
    }
}
