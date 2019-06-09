using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BisLeagues.Core.Models
{
    public partial class Team
    {
        private ICollection<TeamPlayers> _teamPlayers;
        private City _city;
        private County _county;
        private Photo _logo;
        private Player _captainPlayer;
        
        public Team()
        {

        }

        private Team(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }

        public int PhotoId { get; set; }
        public int CityId { get; set; }
        public int CountyId { get; set; }
        public int CaptainPlayerId { get; set; }
        public int Level { get; set; }
        public bool IsActive { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? CreatedOnUtc { get; set; }


        public City City
        {
            get => LazyLoader.Load(this, ref _city);
            set => _city = value;
        }

        public County County
        {
            get => LazyLoader.Load(this, ref _county);
            set => _county = value;
        }
        
        public Photo Logo
        {
            get => LazyLoader.Load(this, ref _logo);
            set => _logo = value;
        }
        
        public Player CaptainPlayer
        {
            get => LazyLoader.Load(this, ref _captainPlayer);
            set => _captainPlayer = value;
        }


        public ICollection<TeamPlayers> TeamPlayers
        {
            get => LazyLoader.Load(this, ref _teamPlayers);
            set => _teamPlayers = value;
        }
    }
}
