using System.Collections.Generic;
using BisLeagues.Core.Models;

namespace BisLeagues.Core.Interfaces.Repositories
{
    public interface IGoalKingRowRepository : IRepository<GoalKingRow>
    {
        IEnumerable<GoalKingRow> GetGoalKingTableRowsBySeasonId(int seasonId);

    }
}
