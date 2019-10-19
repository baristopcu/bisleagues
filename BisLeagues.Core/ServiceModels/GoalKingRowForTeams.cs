using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.ServiceModels
{
    public class GoalKingRowForTeams
    {
        public Team Team { get; set; }
        public int Goals { get; set; }
        
    }
}
