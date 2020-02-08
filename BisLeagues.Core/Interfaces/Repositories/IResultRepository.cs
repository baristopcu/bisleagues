using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Interfaces.Repositories
{
    public interface IResultRepository : IRepository<Result>
    {
        Result GetLastMatchsResultBySeasonId(int seasonId);
        IEnumerable<Result> GetResultsOfMatches(IEnumerable<Match> matches);
    }
}
