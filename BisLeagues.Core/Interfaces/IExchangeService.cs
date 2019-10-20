using System.Collections.Generic;
using BisLeagues.Core.ServiceModels;

namespace BisLeagues.Core.Interfaces
{
    public interface IExchangeService
    {
        List<ExchangeRow> GetTopPlayersInExchange(int seasonId);

    }
}
