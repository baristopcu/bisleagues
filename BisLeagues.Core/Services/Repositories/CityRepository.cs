using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public CityRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }
    }
}
