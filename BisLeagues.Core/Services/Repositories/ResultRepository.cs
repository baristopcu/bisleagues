using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BisLeagues.Core.CustomModels;

namespace BisLeagues.Core.Services.Repositories
{
    public class ResultRepository : Repository<Result>, IResultRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public ResultRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Result GetLastMatchsResultBySeasonId(int seasonId)
        {
            DateTime fromTime = DateTime.UtcNow;
            Result result = _dbContext.Results.Where(x => x.Match.SeasonId == seasonId && x.Match.MatchDate < fromTime && x.Match.IsPlayed == true).OrderByDescending(m => m.Match.MatchDate).FirstOrDefault();
            return result;
        }

        public Result GetLastMatchsResultsBySeasonId(int seasonId)
        {
            DateTime fromTime = DateTime.UtcNow;
            Result result = _dbContext.Results.Where(x => x.Match.SeasonId == seasonId && x.Match.MatchDate < fromTime && x.Match.IsPlayed == true).OrderByDescending(m => m.Match.MatchDate).FirstOrDefault();
            return result;
        }

        public IEnumerable<Result> GetResultsOfMatches(IEnumerable<Match> matches)
        {
            var results = _dbContext.Results.Where(x => matches.Select(y=>y.Id).Contains(x.MatchId));
            return results;
        }

        public IEnumerable<ResultResponseModel> GetResultsOfMatchesForJson()
        {
            var results = _dbContext.Results.Include(x=> x.Match)
            .ThenInclude(x=>x.Home).Include(x=>x.Match).ThenInclude(x=>x.Away)
            .Select(x=> new ResultResponseModel { HomeScore = x.HomeScore, AwayScore =  x.AwayScore, HomeTeamName = x.Match.Home.Name, AwayTeamName = x.Match.Away.Name });
            return results;
        }
    }
}
