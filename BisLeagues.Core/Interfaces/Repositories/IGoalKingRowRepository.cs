using System.Collections.Generic;
using BisLeagues.Core.Models;

namespace BisLeagues.Core.Interfaces.Repositories
{
    public interface IGoalKingRowRepository : IRepository<GoalKingRow>
    {
        IEnumerable<GoalKingRow> GetGoalKingTableRowsBySeasonId(int seasonId);
        IEnumerable<GoalKingRow> GetGoalKingTableRowsBySeasonId(int seasonId, int skip, int take, out int totalCount);

    }
}
