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
        private readonly IPlayerRepository _playerRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMatchRepository _matchRepository;

        public MatchesController(IPlayerRepository playerRepository, ISeasonRepository seasonRepository, IMatchRepository matchRepository) //: base(playerRepository)
        {
            _seasonRepository = seasonRepository;
            _matchRepository = matchRepository;
            _playerRepository = playerRepository;

        }

        // GET: Matches
        public IActionResult Index()
        {
            List<Match> matches = _matchRepository.GetUpcomingMatches().ToList();
            Match upcomingMatch = _matchRepository.GetUpcomingMatch();
            TimeSpan matchCounter = (upcomingMatch.MatchDate - DateTime.Now);
            MatchViewModel model = new MatchViewModel()
            {
                Matches = matches,
                UpComingMatch = upcomingMatch,
                UpComingMatchCounter = matchCounter
            };
            return View(model);
        }
        
    }
}
