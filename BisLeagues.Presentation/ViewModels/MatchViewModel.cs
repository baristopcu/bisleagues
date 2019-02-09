﻿using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.ViewModels
{
    public class MatchViewModel
    {
        public IList<Match> Matches;
        public Match UpComingMatch;
        public TimeSpan UpComingMatchCounter;
    }
}
