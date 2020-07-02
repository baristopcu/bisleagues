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
            List<Match> matches = _matchRepository.GetUpcomingMatchesBySeasonId(UserPreferredSeasonId).ToList();
            Match upcomingMatch = _matchRepository.GetUpcomingMatchBySeasonId(UserPreferredSeasonId);
            TimeSpan matchCounter = new TimeSpan();
            if (upcomingMatch != null)
            {
                matchCounter = (upcomingMatch.MatchDate - DateTime.Now);
            }
            UpComingMatchesViewModel model = new UpComingMatchesViewModel()
            {
                NoMatchFound = matches.Count() == 0,
                Matches = matches,
                UpComingMatch = upcomingMatch,
                UpComingMatchCounter = matchCounter
            };
            return View(model);
        }

        // GET: Matches
        public IActionResult PastMatches()
        {
            List<New> newsOfPastMatches = _newRepository.GetNewsOfPastMatchesBySeasonId(UserPreferredSeasonId).ToList();
            Result lastMatchsResult = _resultRepository.GetLastMatchsResultBySeasonId(UserPreferredSeasonId);
            PastMatchesViewModel model = new PastMatchesViewModel()
            {
                NoMatchFound = newsOfPastMatches.Count() == 0,
                NewsOfPastMatches = newsOfPastMatches,
                LastMatchsResult = lastMatchsResult
            };
            return View(model);
        }

    }
}
