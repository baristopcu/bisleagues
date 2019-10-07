using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Models
{
    public partial class New
    {
        private Team _team;
        private Match _match;
        private Season _season;
        private Photo _videoCoverPhoto;
        private Photo _coverPhoto;

        public New()
        {

        }

        private New(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public string Caption { get; set; }
        public string Content { get; set; }

        public string ShortDescription { get; set; }
        public string VideoUrl { get; set; }
        public int? CoverPhotoId { get; set; }
        public int? VideoCoverPhotoId { get; set; }
        public int? WriterId { get; set; } //TODO: Make it foreign key after writer table ready.
        public int? MatchId { get; set; }
        public int? TeamId { get; set; }
        public int? SeasonId { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public Team Team
        {
            get => LazyLoader.Load(this, ref _team);
            set => _team = value;
        }

        public Match Match
        {
            get => LazyLoader.Load(this, ref _match);
            set => _match = value;
        }

        public Season Season
        {
            get => LazyLoader.Load(this, ref _season);
            set => _season = value;
        }

        public Photo VideoCoverPhoto
        {
            get => LazyLoader.Load(this, ref _videoCoverPhoto);
            set => _videoCoverPhoto = value;
        }

        public Photo CoverPhoto
        {
            get => LazyLoader.Load(this, ref _coverPhoto);
            set => _coverPhoto = value;
        }
    }
}
