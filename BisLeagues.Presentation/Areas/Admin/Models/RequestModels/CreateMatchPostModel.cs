using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Areas.Admin.Models.ViewModels
{
    public class CreateMatchPostModel
    {
        [Required]
        public int HomeId { get; set; }
        [Required]

        public int AwayId { get; set; }
        [Required]

        public DateTime MatchDate { get; set; }
        [Required]

        public TimeSpan MatchHour { get; set; }
    }
}
