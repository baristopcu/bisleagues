using BisLeagues.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Areas.Admin.Models.ViewModels
{
    public class EditMatchPostModel
    {
        [Required]
        public int MatchId { get; set; }

        [Required]
        public int SeasonId { get; set; }

        [Required]
        public int HomeId { get; set; }

        [Required]
        public int AwayId { get; set; }

        public string StadiumName { get; set; }
        public DateTime MatchDate { get; set; }
        public TimeSpan MatchHour { get; set; }

        public int PlayerOfTheMatchId { get; set; }


        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public int HomePoint { get; set; }
        public int AwayPoint { get; set; }

        public List<PlayerId> HomeScorersIds { get; set; }
        public List<PlayerId> AwayScorersIds { get; set; }


        public IFormFile VideoPicture { get; set; }
        public IFormFile NewsPicture { get; set; }


        public string VideoUrl { get; set; }
        public string Caption { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }

    }

    public class PlayerId  
    {
        public int Id { get; set; }
    }

}
