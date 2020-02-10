using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Areas.Admin.Models.ViewModels
{
    public class FilterMatchGetModel
    {
        public int SeasonId { get; set; }
        public DateTime MatchDateFilterStart { get; set; }
        public DateTime MatchDateFilterEnd { get; set; }

        public int TeamId { get; set; }

        public int IsPlayed { get; set; }
    }
}
