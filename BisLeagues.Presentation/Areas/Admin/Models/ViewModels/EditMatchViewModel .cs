using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Areas.Admin.Models.ViewModels
{
    public class EditMatchViewModel
    {
        public Season Season;
        public IEnumerable<Team> Teams;
        public Match Match;
        public List<Player> PlayerList;
    }
}
