using BisLeagues.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BisLeagues.Core.Models
{
    public partial class PointTableRow
    {
        private Season _season;
        private Team _team;

        public PointTableRow()
        {

        }

        private PointTableRow(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public int SeasonId { get; set; }
        public int TeamId { get; set; }
        public int Average { get; set; }
        public int MatchCount { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
        public int Point { get; set; }

        public Season Season
        {
            get => LazyLoader.Load(this, ref _season);
            set => _season = value;
        }
        public Team Team
        {
            get => LazyLoader.Load(this, ref _team);
            set => _team = value;
        }
    }
}
