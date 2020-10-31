using BisLeagues.Core.Data;
using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Core.ServiceModels;
using BisLeagues.Core.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services
{
    public class ExchangeTableService : IExchangeTableService
    {

        private readonly BisLeaguesContext _dbContext;
        private readonly ISeasonRepository _seasonRepository;

        public ExchangeTableService(BisLeaguesContext dbContext, IExchangeTableRowRepository exchangeTableRowRepository, IScoreRepository scoreRepository, ISeasonRepository seasonRepository)
        {
            _dbContext = dbContext;
            _seasonRepository = seasonRepository;
        }


        public async Task<bool> CreateOrUpdateExchangeTablesForActiveSeasons()
        {
            try
            {
                var seasons = _seasonRepository.GetActiveSeasons();
                foreach (var season in seasons)
                {
                    await _dbContext.Database.ExecuteSqlCommandAsync("exec BisUser.UpdateExchangeTableBySeasonId @p0", season.Id);

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
