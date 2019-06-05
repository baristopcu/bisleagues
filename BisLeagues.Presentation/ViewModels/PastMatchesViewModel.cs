using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.ViewModels
{
    public class PastMatchesViewModel
    {
        public IList<Match> Matches;
        public Result LastMatchsResult;
    }
}
