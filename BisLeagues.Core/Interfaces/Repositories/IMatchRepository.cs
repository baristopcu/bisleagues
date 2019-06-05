using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Interfaces.Repositories
{
    public interface IMatchRepository : IRepository<Match>
    {
        IEnumerable<Match> GetMatchesBySeasonId(int seasonId);
        Match GetUpcomingMatch();
        IEnumerable<Match> GetUpcomingMatches();
        IEnumerable<Match> GetUpcomingMatchesByLimit(int limit);
        IEnumerable<Match> GetPastMatches();
        IEnumerable<Match> GetPastMatchesByLimit(int limit);
    }
}
