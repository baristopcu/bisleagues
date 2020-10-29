using BisLeagues.Core.Models;
using BisLeagues.Core.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Models.ViewModels
{
    public class GoalKingViewModel
    {
        public Pagination Pagination;
        public bool NoMatchFound;
        public Result LastMatchsResult;
        public List<GoalKingRow> GoalKingRows;
    }
}
