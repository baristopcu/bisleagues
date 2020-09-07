using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BisLeagues.Presentation.Models;
using BisLeagues.Core.Models;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.BaseControllers;
using BisLeagues.Presentation.Models.ViewModels;
using BisLeagues.Core.Interfaces;
using BisLeagues.Core.ServiceModels;

namespace BisLeagues.Presentation.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly INewRepository _newRepository;
        private readonly IPointTableRowRepository _pointTableRowRepository;
        private readonly IGoalKingRowRepository _goalKingRowRepository;
        private readonly IExchangeService _exchangeService;

        public HomeController(IPlayerRepository playerRepository, ISeasonRepository seasonRepository, IMatchRepository matchRepository, INewRepository newRepository, IPointTableRowRepository pointTableRowRepository, IGoalKingRowRepository goalKingRowRepository, IExchangeService exchangeService, ISettingRepository settingRepository) : base(settingRepository)
        {
            _seasonRepository = seasonRepository;
            _matchRepository = matchRepository;
            _playerRepository = playerRepository;
            _newRepository = newRepository;
            _pointTableRowRepository = pointTableRowRepository;
            _goalKingRowRepository = goalKingRowRepository;
            _exchangeService = exchangeService;
        }

        public IActionResult Index()
        {
            List<Match> upComingMatches = _matchRepository.GetUpcomingMatchesBySeasonIdAndLimit(UserPreferredSeasonId, 5).ToList();
            var upComingMatch = upComingMatches.FirstOrDefault();
            upComingMatches.Remove(upComingMatch);
            TimeSpan matchCounter = upComingMatch != null ? (upComingMatch.MatchDate - DateTime.UtcNow) : new TimeSpan();
            List<GoalKingRow> goalKingPlayersRows = _goalKingRowRepository.GetGoalKingTableRowsBySeasonId(UserPreferredSeasonId).ToList();
            List<ExchangeRow> exchangeTableRows = _exchangeService.GetTopPlayersInExchange(UserPreferredSeasonId);
            List<New> topFiveNews = _newRepository.GetTopNewsBySeasonIdAndLimit(UserPreferredSeasonId, 5).ToList();
            List<Team> topTeams = new List<Team>();

            if (goalKingPlayersRows.Count > 10)
                goalKingPlayersRows = goalKingPlayersRows.Take(10).ToList();

            if (exchangeTableRows.Count > 10)
                exchangeTableRows = exchangeTableRows.Take(10).ToList();

            HomeViewModel model = new HomeViewModel()
            {
                ExchangeTopPlayers = exchangeTableRows,
                GoalKingPlayers = goalKingPlayersRows,
                UpComingMatches = upComingMatches,
                UpComingMatch = upComingMatch,
                UpComingMatchCounter = matchCounter,
                TopNews = topFiveNews,
                TopTeams = topTeams
            };
            return View(model);
        }
    }
}
