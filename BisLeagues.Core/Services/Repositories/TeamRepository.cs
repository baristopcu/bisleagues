using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services.Repositories
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public TeamRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public IEnumerable<Team> GetTeamsWaitingConfirm()
        {
            IEnumerable<Team> teams = _dbContext.Teams.Where(x=>x.IsActive == false);
            return teams;
        }
    }
}
