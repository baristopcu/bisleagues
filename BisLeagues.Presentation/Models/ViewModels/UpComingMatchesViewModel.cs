using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Models.ViewModels
{
    public class UpComingMatchesViewModel
    {
        public IList<Match> Matches;
        public Match UpComingMatch;
        public TimeSpan UpComingMatchCounter;
    }
}
