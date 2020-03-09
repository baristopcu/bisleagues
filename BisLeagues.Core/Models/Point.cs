using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BisLeagues.Core.Models
{
    public partial class Point
    {
        private Season _season;
        private Result _result;
        
        public Point()
        {

        }

        private Point(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public int SeasonId { get; set; }
        public int ResultId { get; set; }
        public int HomePoint { get; set; }
        public int AwayPoint { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime CreatedOnUtc { get; set; }


        public Season Season
        {
            get => LazyLoader.Load(this, ref _season);
            set => _season = value;
        }

        public Result Result
        {
            get => LazyLoader.Load(this, ref _result);
            set => _result = value;
        }
    }
}
