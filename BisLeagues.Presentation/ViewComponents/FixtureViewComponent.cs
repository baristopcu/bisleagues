using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.ViewComponents
{
    public class FixtureViewComponent : ViewComponent
    {
        private readonly IMatchRepository _matchRepository;

        public FixtureViewComponent(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public IViewComponentResult Invoke(int numberOfItems)
        {
            int selectedSeasonId = Request.Cookies["SelectedSeasonId"] != null ? int.Parse(Request.Cookies["SelectedSeasonId"]) : 0;
            var matches = _matchRepository.GetUpcomingMatchesBySeasonIdAndLimit(selectedSeasonId,numberOfItems);
            var upComingMatchesPartOne = matches.Take(numberOfItems/2);
            var upComingMatchesPartTwo = matches.OrderByDescending(x=>x.Id).Take(numberOfItems/2);
            FixtureViewModel model = new FixtureViewModel() {
                MatchesPartOne = upComingMatchesPartOne,
                MatchesPartTwo = upComingMatchesPartTwo
            };
            return View(model);
        }
    }
}
