using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Interfaces.Repositories
{
    public interface INewRepository : IRepository<New>
    {
        New GetNewByMatchId(int matchId);
        IEnumerable<New> GetTopNewsBySeasonIdAndLimit(int seasonId, int limit);
        IEnumerable<New> GetNewsOfPastMatchesBySeasonId(int seasonId);
        IEnumerable<New> GetNewsOfPastMatchesBySeasonId(int seasonId, int skip, int take, out int totalCount);
        IEnumerable<New> GetNewsOfPastMatchesBySeasonIdAndLimit(int seasonId, int limit);
        IEnumerable<New> GetGeneralNews();
        IEnumerable<New> GetNewsBySeasonAndMatchIds(int seasonId, List<int> matchIds);
    }
}
