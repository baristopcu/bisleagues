using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services.Repositories
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public PlayerRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public IEnumerable<Player> ListAll()
        {
            IEnumerable<Player> players = _dbContext.Players;
            //IList<Player> players = _dbContext.Player
            //     .Select(x => new Player { Id = x.Id, Name = x.Name, Surname = x.Surname, BirthDate = x.BirthDate, Email = x.Email, TeamPlayers = new Team { Name = new string { x.TeamPlayers.Select(y => y.Team.Name) } } } ).ToList();
            return players;
        }
    }
}
