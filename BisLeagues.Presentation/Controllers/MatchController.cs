using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.Models.ViewModels;
using BisLeagues.Presentation.BaseControllers;

namespace BisLeagues.Presentation.Controllers
{
    public class MatchController : BaseController<MatchController>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IResultRepository _resultRepository;
        private readonly INewRepository _newRepository;

        public MatchController(ISeasonRepository seasonRepository, IMatchRepository matchRepository, IResultRepository resultRepository, INewRepository newRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _seasonRepository = seasonRepository;
            _matchRepository = matchRepository;
            _resultRepository = resultRepository;
            _newRepository = newRepository;

        }

        // GET: Matches
        public IActionResult UpComingMatches()
        {
            int selectedSeasonId = Request.Cookies["SelectedSeasonId"] != null ? int.Parse(Request.Cookies["SelectedSeasonId"]) : 0;
            List<Match> matches = _matchRepository.GetUpcomingMatchesBySeasonId(selectedSeasonId).ToList();
            Match upcomingMatch = _matchRepository.GetUpcomingMatchBySeasonId(selectedSeasonId);
            TimeSpan matchCounter = new TimeSpan();
            if (upcomingMatch != null)
            {
                matchCounter = (upcomingMatch.MatchDate - DateTime.Now);
            }
            UpComingMatchesViewModel model = new UpComingMatchesViewModel()
            {
                NoMatchFound = true,
                Matches = matches,
                UpComingMatch = upcomingMatch,
                UpComingMatchCounter = matchCounter
            };
            return View(model);
        }

        // GET: Matches
        public IActionResult PastMatches()
        {
            int selectedSeasonId = Request.Cookies["SelectedSeasonId"] != null ? int.Parse(Request.Cookies["SelectedSeasonId"]) : 0;
            List<New> newsOfPastMatches = _newRepository.GetNewsOfPastMatchesBySeasonId(selectedSeasonId).ToList();
            Result lastMatchsResult = _resultRepository.GetLastMatchsResultBySeasonId(selectedSeasonId);
            PastMatchesViewModel model = new PastMatchesViewModel()
            {
                NoMatchFound = lastMatchsResult == null,
                NewsOfPastMatches = newsOfPastMatches,
                LastMatchsResult = lastMatchsResult
            };
            return View(model);
        }

    }
}
