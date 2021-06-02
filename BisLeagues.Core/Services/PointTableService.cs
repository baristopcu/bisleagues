using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Core.ServiceModels;
using BisLeagues.Core.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using BisLeagues.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace BisLeagues.Core.Services
{
    public class PointTableService : IPointTableService
    {
        private readonly BisLeaguesContext _dbContext;
        private readonly ISeasonRepository _seasonRepository;

        public PointTableService(BisLeaguesContext bisLeaguesContext, ISeasonRepository seasonRepository)
        {
            _dbContext = bisLeaguesContext;
            _seasonRepository = seasonRepository;
        }

        public async Task<bool> CreateOrUpdatePointTablesForActiveSeasons()
        {
            try
            {
                var seasons = _seasonRepository.GetActiveSeasons();
                foreach (var season in seasons)
                {
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec UpdatePointTableBySeasonId @p0", season.Id);
                   
                }

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}