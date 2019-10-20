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

        public GoalKingService(IScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }


        public List<GoalKingRowForPlayers> GetGoalKingsBySeasonId(int seasonId)
        {
            var scores = _scoreRepository.GetAllScoresBySeasonId(seasonId);
            IEnumerable<GoalKingRowForPlayers> goalKingRows = scores.GroupBy(x => x.Player).Select(g => new GoalKingRowForPlayers { Player = g.Key, Goals = g.Sum(x=>x.Goals) }).OrderByDescending(x=>x.Goals);
            return goalKingRows.ToList();
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
            int goals = scores.Where(x=>x.ScoredTeamId == teamId).GroupBy(x => x.ScoredTeam).Select(g=> g.Sum(x => x.Goals)).FirstOrDefault();
            return goals;
        }
    }
}
