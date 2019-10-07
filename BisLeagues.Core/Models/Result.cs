using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BisLeagues.Core.Models
{
    public partial class Result
    {
        private Match _match;

        public Result()
        {
        }

        private Result(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public int MatchId { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        
        public Match Match
        {
            get => LazyLoader.Load(this, ref _match);
            set => _match = value;
        }
    }
}
