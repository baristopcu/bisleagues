using BisLeagues.Core.Models;
using BisLeagues.Core.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<ExchangeRow> ExchangeTopPlayers { get; set; }
        public List<GoalKingRowForPlayers> GoalKingPlayers { get; set; }
        public List<Match> UpComingMatches { get; set; }
        public Match UpComingMatch { get; set; }
        public TimeSpan UpComingMatchCounter { get; set; }
        public List<New> TopNews { get; set; }
        public List<Team> TopTeams { get; set; }
    }
}
