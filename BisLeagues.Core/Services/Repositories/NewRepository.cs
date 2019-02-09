using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services.Repositories
{
    public class NewRepository : Repository<New>, INewRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public NewRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public IEnumerable<New> GetTopNewsByLimit(int limit)
        {
            return _dbContext.News.OrderByDescending(x => x.Id).Take(5);
        }
    }
}
