using System.Collections.Generic;
using System.Threading.Tasks;
using BisLeagues.Core.ServiceModels;

namespace BisLeagues.Core.Interfaces
{
    public interface IGoalKingService
    {
        Task<bool> CreateGoalKingTablesForActiveSeasons();
        //List<GoalKingRowForPlayers> GetGoalKingsBySeasonId(int seasonId);

        int GetPlayersGoalsByPlayerIdAndSeasonId(int playerId, int seasonId);

        int GetTeamGoalsByTeamIdAndSeasonId(int teamId, int seasonId);
    }
}
