using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Areas.Admin.Models.ViewModels
{
    public class CreateNewViewModel
    {
        public IEnumerable<Season> Seasons;
        public New New;
    }
}
