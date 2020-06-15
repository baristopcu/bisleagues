using BisLeagues.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Areas.Admin.Models.ViewModels
{
    public class CreateNewPostModel
    {

        [Required]
        public int SeasonId { get; set; }
        public List<PlayerId> HomeScorersIds { get; set; }
        public List<PlayerId> AwayScorersIds { get; set; }


        public IFormFile VideoPicture { get; set; }
        public IFormFile NewsPicture { get; set; }


        public string VideoUrl { get; set; }
        public string Caption { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }

    }
}
