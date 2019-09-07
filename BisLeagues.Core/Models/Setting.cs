using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BisLeagues.Core.Models
{
    public partial class Setting
    {

        private Setting(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

    }
    
}
