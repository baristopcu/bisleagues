using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services.Repositories
{
    public class SeasonRepository : Repository<Season>, ISeasonRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public SeasonRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public Season GetActiveSeason()
        {
            var activeSeason = _dbContext.Seasons.Where(x => x.Active == true).FirstOrDefault();
            return activeSeason;
        }

        public int GetActiveSeasonId()
        {
            var activeSeasonId = _dbContext.Seasons.Where(x => x.Active == true).Select(x=>x.Id).FirstOrDefault();
            return activeSeasonId;
        }

        public IEnumerable<Season> ListAll()
        {
            IEnumerable<Season> seasons =  _dbContext.Seasons;
            //IList<Player> players = _dbContext.Player
            //     .Select(x => new Player { Id = x.Id, Name = x.Name, Surname = x.Surname, BirthDate = x.BirthDate, Email = x.Email, TeamPlayers = new Team { Name = new string { x.TeamPlayers.Select(y => y.Team.Name) } } } ).ToList();
            return seasons;
        }
    }
}
