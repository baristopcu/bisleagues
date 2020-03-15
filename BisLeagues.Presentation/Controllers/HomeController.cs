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
        private readonly IPointTableService _pointTableService;
        private readonly IGoalKingService _goalKingService;
        private readonly IExchangeService _exchangeService;

        public HomeController(IPlayerRepository playerRepository, ISeasonRepository seasonRepository, IMatchRepository matchRepository, INewRepository newRepository, IPointTableService pointTableService, IGoalKingService goalKingService, IExchangeService exchangeService, ISettingRepository settingRepository) : base(settingRepository)
        {
            _seasonRepository = seasonRepository;
            _matchRepository = matchRepository;
            _playerRepository = playerRepository;
            _newRepository = newRepository;
            _pointTableService = pointTableService;
            _goalKingService = goalKingService;
            _exchangeService = exchangeService;
        }

        public IActionResult Index()
        {
            List<Match> upComingMatches = _matchRepository.GetUpcomingMatchesBySeasonIdAndLimit(UserPreferredSeasonId, 5).ToList();
            var upComingMatch = upComingMatches.FirstOrDefault();
            upComingMatches.Remove(upComingMatch);
            TimeSpan matchCounter = upComingMatch != null ? (upComingMatch.MatchDate - DateTime.UtcNow) : new TimeSpan();
            List<GoalKingRowForPlayers> goalKingPlayersRows = _goalKingService.GetGoalKingsBySeasonId(UserPreferredSeasonId);
            List<ExchangeRow> exchangeTableRows = _exchangeService.GetTopPlayersInExchange(UserPreferredSeasonId);
            List<New> topFiveNews = _newRepository.GetTopNewsBySeasonIdAndLimit(UserPreferredSeasonId, 5).ToList();
            List<Team> topTeams = _pointTableService.GetPointTableBySeasonId(UserPreferredSeasonId).Count > 5 ? _pointTableService.GetPointTableBySeasonId(UserPreferredSeasonId).GetRange(0, 5).Select(x=>x.Team).ToList(): null;

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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
