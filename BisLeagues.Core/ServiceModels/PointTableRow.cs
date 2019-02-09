using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.ServiceModels
{
    public class PointTableRow
    {
        public Team Team { get; set; }
        public int Average { get; set; }
        public int MatchCount { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
        public int Point { get; set; }
    }
}
