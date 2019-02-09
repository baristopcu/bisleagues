using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.ViewModels
{
    public class FixtureViewModel
    {
        public IEnumerable<Match> MatchesPartOne { get; set; }
        public IEnumerable<Match> MatchesPartTwo { get; set; }
    }
}
