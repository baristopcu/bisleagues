using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Models.ViewModels
{
    public class TeamDetailViewModel
    {
        public Team Team { get; set; }
        public int TotalGoalCount { get; set; }
        public IEnumerable<TransferRequest> IncomingTransferRequests { get; set; }
        public IEnumerable<TransferRequest> OutgoingTransferRequests { get; set; }
        public IEnumerable<New> PastMatchesNews { get; set; }
        public IEnumerable<Match> UpcomingMatches { get; set; }
    }
}
