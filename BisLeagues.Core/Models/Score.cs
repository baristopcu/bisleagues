using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BisLeagues.Core.Models
{
    public partial class Score
    {
        private Result _result;
        private Player _player;
        private Team _scoredTeam;

        public Score()
        {
        }

        private Score(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public int ResultId { get; set; }
        public int PlayerId { get; set; }
        public int ScoredTeamId { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }

        public Result Result
        {
            get => LazyLoader.Load(this, ref _result);
            set => _result = value;
        }

        public Player Player
        {
            get => LazyLoader.Load(this, ref _player);
            set => _player = value;
        }

        public Team ScoredTeam
        {
            get => LazyLoader.Load(this, ref _scoredTeam);
            set => _scoredTeam = value;
        }
    }
    
}
