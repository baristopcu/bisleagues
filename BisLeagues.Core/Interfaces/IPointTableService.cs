using System.Collections.Generic;
using BisLeagues.Core.ServiceModels;

namespace BisLeagues.Core.Interfaces
{
    public interface IPointTableService
    {
        List<PointTableRow> GetPointTableBySeasonId(int seasonId);
    }
}
