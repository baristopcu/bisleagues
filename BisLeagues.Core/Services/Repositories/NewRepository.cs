﻿using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BisLeagues.Core.Services.Repositories
{
    public class NewRepository : Repository<New>, INewRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public NewRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public New GetNewByMatchId(int matchId)
        {
            return _dbContext.News.Where(x => x.MatchId == matchId).SingleOrDefault();
        }

        public IEnumerable<New> GetTopNewsBySeasonIdAndLimit(int seasonId, int limit)
        {
            return _dbContext.News.Where(x=>x.SeasonId==seasonId).OrderByDescending(x => x.Id).Take(limit);
        }

        public IEnumerable<New> GetNewsOfPastMatchesBySeasonId(int seasonId)
        {
            IEnumerable<New> newsOfMatches = _dbContext.News.Include(x=>x.Match).Where(x => x.SeasonId == seasonId && x.Match.IsPlayed == true && x.Match.MatchDate < DateTime.UtcNow).OrderByDescending(p => p.Match.MatchDate);
            return newsOfMatches;
        }
        public IEnumerable<New> GetNewsOfPastMatchesBySeasonId(int seasonId, int skip, int take, out int totalCount)
        {
            IEnumerable<New> newsOfMatches = _dbContext.News.Where(x => x.SeasonId == seasonId && x.Match.IsPlayed == true && x.Match.MatchDate < DateTime.UtcNow).OrderByDescending(p => p.Match.MatchDate);
            totalCount = newsOfMatches.Count();
            newsOfMatches = newsOfMatches.Skip(skip).Take(take);
            return newsOfMatches;
        }

        public IEnumerable<New> GetNewsOfPastMatchesBySeasonIdAndLimit(int seasonId, int limit)
        {
            IEnumerable<New> newsOfMatches = _dbContext.News.Where(x => x.SeasonId == seasonId && x.Match.IsPlayed == true && x.Match.MatchDate < DateTime.UtcNow).OrderByDescending(p => p.Match.MatchDate).Take(limit);
            return newsOfMatches;
        }

        public IEnumerable<New> GetGeneralNews()
        {
            IEnumerable<New> newsOfMatches = _dbContext.News.Where(x => x.MatchId == null).OrderByDescending(p => p.CreatedOnUtc);
            return newsOfMatches;
        }
        
        public IEnumerable<New> GetNewsBySeasonAndMatchIds(int seasonId, List<int> matchIds)
        {
            IEnumerable<New> newsOfMatches = _dbContext.News.Where(x => x.SeasonId == seasonId &&  matchIds.Any(k => 
            k == x.MatchId)).Include(x=>x.Match).OrderByDescending(p => p.CreatedOnUtc);
            return newsOfMatches;
        }
    }
}
