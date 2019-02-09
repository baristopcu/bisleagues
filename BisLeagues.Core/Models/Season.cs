using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BisLeagues.Core.Models
{
    public partial class Season
    {
        private ICollection<Match> _matches;

        private Season(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; }

        [JsonProperty("matches")]
        public ICollection<Match> Matches
        {
            get => LazyLoader.Load(this, ref _matches);
            set => _matches = value;
        }
    }
}
