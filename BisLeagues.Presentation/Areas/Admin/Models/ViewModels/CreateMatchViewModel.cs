﻿using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Areas.Admin.Models.ViewModels
{
    public class CreateMatchViewModel
    {
        public IEnumerable<Season> Seasons;
        public IEnumerable<Team> Teams;
    }
}
