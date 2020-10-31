using System.Collections.Generic;
using System.Threading.Tasks;
using BisLeagues.Core.ServiceModels;

namespace BisLeagues.Core.Interfaces
{
    public interface IGoalKingService
    {
        Task<bool> CreateOrUpdateGoalKingTablesForActiveSeasons();

        int GetPlayersGoalsByPlayerIdAndSeasonId(int playerId, int seasonId);

        int GetTeamGoalsByTeamIdAndSeasonId(int teamId, int seasonId);
    }
}
