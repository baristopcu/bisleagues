using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Core.ServiceModels;
using BisLeagues.Core.Services.Repositories;
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

        public GoalKingService(IScoreRepository scoreRepository, ISeasonRepository seasonRepository, IGoalKingRowRepository goalKingRowRepository)
        {
            _scoreRepository = scoreRepository;
            _seasonRepository = seasonRepository;
            _goalKingRowRepository = goalKingRowRepository;
        }


        public async Task<bool> CreateGoalKingTablesForActiveSeasons()
        {
            try
            {
                var activeSeasons = _seasonRepository.GetActiveSeasons();
                foreach (var season in activeSeasons)
                {
                    int seasonId = season.Id;
                    var oldGoalKingTable = _goalKingRowRepository.GetGoalKingTableRowsBySeasonId(seasonId);
                    _goalKingRowRepository.RemoveRange(oldGoalKingTable);
                    var scores = _scoreRepository.GetAllScoresBySeasonId(seasonId);
                    IEnumerable<GoalKingRow> goalKingRows = scores.GroupBy(x => x.Player).Select(g => new GoalKingRow {  SeasonId = seasonId, Player = g.Key, Goals = g.Sum(x => x.Goals) }).OrderByDescending(x => x.Goals);
                    _goalKingRowRepository.AddRange(goalKingRows);
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
            var scores = _scoreRepository.GetAllScoresBySeasonId(seasonId);
            int goals = scores.Where(x => x.PlayerId == playerId).GroupBy(x => x.Player).Select(g => g.Sum(x => x.Goals)).FirstOrDefault();
            return goals;
        }

        public int GetTeamGoalsByTeamIdAndSeasonId(int teamId, int seasonId)
        {
            var scores = _scoreRepository.GetAllScoresBySeasonId(seasonId);
            int goals = scores.Where(x => x.ScoredTeamId == teamId).GroupBy(x => x.ScoredTeam).Select(g => g.Sum(x => x.Goals)).FirstOrDefault();
            return goals;
        }
    }
}
