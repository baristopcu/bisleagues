using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.ViewModels
{
    public class HomeViewModel
    {
        public Match UpComingMatch { get; set; }
        public TimeSpan UpComingMatchCounter { get; set; }
        public List<New> TopNews { get; set; }
        public List<Team> TopTeams { get; set; }
    }
}
