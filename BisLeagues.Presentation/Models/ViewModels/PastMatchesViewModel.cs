﻿using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Models.ViewModels
{
    public class PastMatchesViewModel
    {
        public Pagination Pagination;
        public bool NoMatchFound;
        public IList<New> NewsOfPastMatches;
        public Result LastMatchsResult;
    }
}
