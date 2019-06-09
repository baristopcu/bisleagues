using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Models.ViewModels
{
    public class TeamApplicationViewModel
    {
        public IEnumerable<City> Cities { get; set; }
    }
}
