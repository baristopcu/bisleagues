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
    public class ExchangeService : ExchangeRow, IExchangeService
    {
        private readonly IScoreRepository _scoreRepository;

        public ExchangeService(IScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }

        public List<ExchangeRow> GetTopPlayersInExchange(int seasonId)
        {
            var scores = _scoreRepository.GetAllScoresBySeasonId(seasonId);
            IEnumerable<ExchangeRow> exchangeRows = scores.GroupBy(x => x.Result.PlayerOfTheMatch).Select(g => new ExchangeRow { Player = g.Key, Value = g.Count()*10 }).OrderByDescending(x => x.Value);
            return exchangeRows.ToList();
        }
        public List<ExchangeRow> GetTopPlayersInExchange(int seasonId, int skip, int take, out int totalCount)
        {
            var scores = _scoreRepository.GetAllScoresBySeasonId(seasonId);
            IEnumerable<ExchangeRow> exchangeRows = scores.GroupBy(x => x.Result.PlayerOfTheMatch).Select(g => new ExchangeRow { Player = g.Key, Value = g.Count() * 10 }).OrderByDescending(x => x.Value);
            totalCount = exchangeRows.Count();
            return exchangeRows.Skip(skip).Take(take).ToList();
        }
    }
}
