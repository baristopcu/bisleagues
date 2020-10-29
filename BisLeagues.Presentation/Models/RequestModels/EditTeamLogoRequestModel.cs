using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Models.RequestModels
{

    public class EditTeamLogoRequestModel
    {
        public int TeamId { get; set; }
        public IFormFile TeamLogo { get; set; }

    }
}
