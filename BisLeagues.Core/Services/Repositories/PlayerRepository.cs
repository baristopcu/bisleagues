﻿using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services.Repositories
{
    public class ScoreRepository : Repository<Score>, IScoreRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public ScoreRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public IEnumerable<Score> ListAll()
        {
            IEnumerable<Score> scores = _dbContext.Scores;
            //IList<Player> players = _dbContext.Player
            //     .Select(x => new Player { Id = x.Id, Name = x.Name, Surname = x.Surname, BirthDate = x.BirthDate, Email = x.Email, TeamPlayers = new Team { Name = new string { x.TeamPlayers.Select(y => y.Team.Name) } } } ).ToList();
            return scores;
        }
    }
}
