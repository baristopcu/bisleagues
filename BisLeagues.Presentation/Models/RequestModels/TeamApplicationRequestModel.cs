using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Models.RequestModels
{
    public class TeamApplicationRequestModel
    {
        public string TeamName { get; set; }
        public int City { get; set; }
        public int County { get; set; }
        public IFormFile Logo { get; set; }
    }
}
