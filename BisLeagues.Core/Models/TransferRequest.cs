using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BisLeagues.Core.Models
{
    public partial class TransferRequest
    {

        private Team _team;
        private Player _player;

        private TransferRequest(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }

        public Team Team
        {
            get => LazyLoader.Load(this, ref _team);
            set => _team = value;
        }

        public Player Player
        {
            get => LazyLoader.Load(this, ref _player);
            set => _player = value;
        }


    }

}
