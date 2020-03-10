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

namespace BisLeagues.Presentation.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly INewRepository _newRepository;
        private readonly IPointTableService _pointTableService;

        public HomeController(IPlayerRepository playerRepository, ISeasonRepository seasonRepository, IMatchRepository matchRepository, INewRepository newRepository, IPointTableService pointTableService, ISettingRepository settingRepository) : base(settingRepository)
        {
            _seasonRepository = seasonRepository;
            _matchRepository = matchRepository;
            _playerRepository = playerRepository;
            _newRepository = newRepository;
            _pointTableService = pointTableService;

        }

        public IActionResult Index()
        {
            List<Match> upComingMatches = _matchRepository.GetUpcomingMatchesBySeasonIdAndLimit(UserPreferredSeasonId, 5).ToList();
            var upComingMatch = upComingMatches.FirstOrDefault();
            upComingMatches.Remove(upComingMatch);
            TimeSpan matchCounter = upComingMatch != null ? (upComingMatch.MatchDate - DateTime.UtcNow) : new TimeSpan();
            List<New> topFiveNews = _newRepository.GetTopNewsBySeasonIdAndLimit(UserPreferredSeasonId, 5).ToList();
            List<Team> topTeams = _pointTableService.GetPointTableBySeasonId(UserPreferredSeasonId).Count > 5 ? _pointTableService.GetPointTableBySeasonId(UserPreferredSeasonId).GetRange(0, 5).Select(x=>x.Team).ToList(): null;
            HomeViewModel model = new HomeViewModel()
            {
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
