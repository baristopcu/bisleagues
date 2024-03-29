﻿using BisLeagues.Core.Models;
using BisLeagues.Core.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<ExchangeTableRow> ExchangeTopPlayers { get; set; }
        public List<GoalKingRow> GoalKingPlayers { get; set; }
        public List<UpComingMatchModel> UpComingMatches { get; set; }
        public Match UpComingMatch { get; set; }
        public TimeSpan UpComingMatchCounter { get; set; }
        public List<New> TopNews { get; set; }
        public List<Team> TopTeams { get; set; }
    }
}
