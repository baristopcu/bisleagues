using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Areas.Admin.Models.ViewModels
{
    public class FilterNewGetModel
    {
        public DateTime DateFilterStart { get; set; }
        public DateTime DateFilterEnd { get; set; }
    }
}
