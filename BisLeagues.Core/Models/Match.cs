﻿using BisLeagues.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BisLeagues.Core.Models
{
    public partial class Match
    {
        private Season _season;
        private Team _home;
        private Team _away;
        private Result _result;
        private New _new;

        public Match()
        {

        }

        private Match(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public int SeasonId { get; set; }
        public int HomeId { get; set; }
        public int AwayId { get; set; }
        public string StadiumName { get; set; }
        public bool IsPlayed { get; set; }
        public DateTime MatchDate { get; set; }

        public Season Season
        {
            get => LazyLoader.Load(this, ref _season);
            set => _season = value;
        }
        public Team Home
        {
            get => LazyLoader.Load(this, ref _home);
            set => _home = value;
        }
        public Team Away
        {
            get => LazyLoader.Load(this, ref _away);
            set => _away = value;
        }
        public Result Result
        {
            get => LazyLoader.Load(this, ref _result);
            set => _result = value;
        }
        public New New
        {
            get => LazyLoader.Load(this, ref _new);
            set => _new = value;
        }
    }
}
