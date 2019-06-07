using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BisLeagues.Core.Models
{
    public partial class Player
    {
        private ICollection<TeamPlayers> _teamPlayers;
        private User _user;

        public Player()
        {

        }

        private Player(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public DateTime? BirthDate { get; set; }
        public int UserId { get; set; }

        [JsonProperty("teams")]
        public ICollection<TeamPlayers> TeamPlayers
        {
            get => LazyLoader.Load(this, ref _teamPlayers);
            set => _teamPlayers = value;
        }

        public User User
        {
            get => LazyLoader.Load(this, ref _user);
            set => _user = value;
        }
    }
}
