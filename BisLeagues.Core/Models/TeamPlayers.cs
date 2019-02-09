
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BisLeagues.Core.Models
{
    public partial class TeamPlayers
    {

        private Team _team;
        private Player _player;

        public TeamPlayers()
        {
        }

        private TeamPlayers(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }
        [Key]
        public int TeamId { get; set; }
        [Key]
        public int PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public Player Player
        {
            get => LazyLoader.Load(this, ref _player);
            set => _player = value;
        }
        [ForeignKey("TeamId")]
        public Team Team
        {
            get => LazyLoader.Load(this, ref _team);
            set => _team = value;
        }
    }
}
