using System.Collections.Generic;
using BisLeagues.Core.ServiceModels;

namespace BisLeagues.Core.Interfaces
{
    public interface IGoalKingService
    {
        List<GoalKingRowForPlayers> GetGoalKingsBySeasonId(int seasonId);

        int GetPlayersGoalsByPlayerIdAndSeasonId(int playerId, int seasonId);

        int GetTeamGoalsByTeamIdAndSeasonId(int teamId, int seasonId);
    }
}
