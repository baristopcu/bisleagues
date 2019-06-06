using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public IEnumerable<New> GetTopNewsByLimit(int limit)
        {
            return _dbContext.News.OrderByDescending(x => x.Id).Take(limit);
        }

        public IEnumerable<New> GetNewsOfPastMatches()
        {
            IEnumerable<New> newsOfMatches = _dbContext.News.Where(x => x.Match.IsPlayed == true && x.Match.MatchDate < DateTime.UtcNow).OrderByDescending(p => p.Match.MatchDate);
            return newsOfMatches;
        }

        public IEnumerable<New> GetNewsOfPastMatchesByLimit(int limit)
        {
            IEnumerable<New> newsOfMatches = _dbContext.News.Where(x => x.Match.IsPlayed == true && x.Match.MatchDate < DateTime.UtcNow).OrderByDescending(p => p.Match.MatchDate).Take(limit);
            return newsOfMatches;
        }
    }
}
