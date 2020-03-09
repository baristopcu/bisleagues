using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Areas.Admin.Models.ViewModels
{
    public class EditMatchViewModel
    {
        public List<Season> Seasons;
        public IEnumerable<Team> Teams;
        public Match Match;
        public Point Point;
        public List<Player> PlayerList;
    }
}
