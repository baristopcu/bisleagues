using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Interfaces.Repositories
{
    public interface ISeasonRepository : IRepository<Season>
    {
        IEnumerable<Season> ListAll();
        Season GetActiveSeason();
        int GetActiveSeasonId();
    }

}
