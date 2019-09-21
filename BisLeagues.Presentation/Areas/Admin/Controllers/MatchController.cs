using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.Areas.Admin.BaseControllers;
using BisLeagues.Presentation.Areas.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BisLeagues.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MatchController : BaseController<MatchController>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly ITeamRepository _teamRepository;
        public MatchController(ISeasonRepository seasonRepository, ITeamRepository teamRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _seasonRepository = seasonRepository;
            _teamRepository = teamRepository;

        }
        public IActionResult Create()
        {
            var season = _seasonRepository.GetActiveSeason();
            var teams = _teamRepository.GetAll();
            CreateMatchViewModel model = new CreateMatchViewModel() {
                Season = season,
                Teams = teams
            };
            return View(model);
        }
    }
}