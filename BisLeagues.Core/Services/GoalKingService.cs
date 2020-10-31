using BisLeagues.Core.Data;
using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Core.ServiceModels;
using BisLeagues.Core.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services
{
    public class GoalKingService : GoalKingRowForPlayers, IGoalKingService
    {
        private readonly IScoreRepository _scoreRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IGoalKingRowRepository _goalKingRowRepository;
        private readonly BisLeaguesContext _dbContext;

        public GoalKingService(IScoreRepository scoreRepository, ISeasonRepository seasonRepository, IGoalKingRowRepository goalKingRowRepository, BisLeaguesContext dbContext)
        {
            _scoreRepository = scoreRepository;
            _seasonRepository = seasonRepository;
            _goalKingRowRepository = goalKingRowRepository;
            _dbContext = dbContext;
        }


        public async Task<bool> CreateOrUpdateGoalKingTablesForActiveSeasons()
        {
            try
            {
                var seasons = _seasonRepository.GetActiveSeasons();
                foreach (var season in seasons)
                {
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec BisUser.UpdateGoalKingTableBySeasonId @p0", season.Id);

                }
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public int GetPlayersGoalsByPlayerIdAndSeasonId(int playerId, int seasonId)
        {
            var goals = _goalKingRowRepository.Find(x => x.PlayerId == playerId && x.SeasonId == seasonId).FirstOrDefault()?.Goals;
            return goals.HasValue ? goals.Value : 0;
        }

        public int GetTeamGoalsByTeamIdAndSeasonId(int teamId, int seasonId)
        {
            var scores = _scoreRepository.GetAllScoresBySeasonId(seasonId);
            int goals = scores.Where(x => x.ScoredTeamId == teamId).GroupBy(x => x.ScoredTeam).Select(g => g.Sum(x => x.Goals)).FirstOrDefault();
            return goals;
        }
    }
}
