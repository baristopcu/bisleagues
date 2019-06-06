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
using BisLeagues.Presentation.ViewModels;

namespace BisLeagues.Presentation.Controllers
{
    public class MatchesController : Controller
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IResultRepository _resultRepository;
        private readonly INewRepository _newRepository;

        public MatchesController(ISeasonRepository seasonRepository, IMatchRepository matchRepository, IResultRepository resultRepository, INewRepository newRepository) //: base(playerRepository)
        {
            _seasonRepository = seasonRepository;
            _matchRepository = matchRepository;
            _resultRepository = resultRepository;
            _newRepository = newRepository;

        }

        // GET: Matches
        public IActionResult UpComingMatches()
        {
            List<Match> matches = _matchRepository.GetUpcomingMatches().ToList();
            Match upcomingMatch = _matchRepository.GetUpcomingMatch();
            TimeSpan matchCounter = (upcomingMatch.MatchDate - DateTime.Now);
            UpComingMatchesViewModel model = new UpComingMatchesViewModel()
            {
                Matches = matches,
                UpComingMatch = upcomingMatch,
                UpComingMatchCounter = matchCounter
            };
            return View(model);
        }

        // GET: Matches
        public IActionResult PastMatches()
        {
            List<New> newsOfPastMatches = _newRepository.GetNewsOfPastMatches().ToList();
            Result lastMatchsResult = _resultRepository.GetLastMatchsResult();
            PastMatchesViewModel model = new PastMatchesViewModel()
            {
                NewsOfPastMatches = newsOfPastMatches,
                LastMatchsResult = lastMatchsResult
            };
            return View(model);
        }

    }
}
