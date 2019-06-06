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
        IEnumerable<New> GetTopNewsByLimit(int limit);
        IEnumerable<New> GetNewsOfPastMatches();
        IEnumerable<New> GetNewsOfPastMatchesByLimit(int limit);
    }
}
