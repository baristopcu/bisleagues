using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BisLeagues.Core.CustomModels
{
    public partial class ResultResponseModel
    {
        public string HomeTeamName { get; set; }
        public int HomeScore { get; set; }
        public string AwayTeamName { get; set; }
        public int AwayScore { get; set; }
    }
}
