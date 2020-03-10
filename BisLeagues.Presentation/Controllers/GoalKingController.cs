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
using BisLeagues.Core.ServiceModels;
using BisLeagues.Core.Interfaces;
using BisLeagues.Presentation.BaseControllers;

namespace BisLeagues.Presentation.Controllers
{
    public class GoalKingController : BaseController<GoalKingController>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IGoalKingService _goalKingService;
        private readonly IResultRepository _resultRepository;
        public GoalKingController(ISeasonRepository seasonRepository, IGoalKingService goalKingService, IResultRepository resultRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _seasonRepository = seasonRepository;
            _goalKingService = goalKingService;
            _resultRepository = resultRepository;
        }

        public IActionResult Index()
        {
            var lastMatchsResult = _resultRepository.GetLastMatchsResultBySeasonId(UserPreferredSeasonId);
            List<GoalKingRowForPlayers> goalKingRows = _goalKingService.GetGoalKingsBySeasonId(UserPreferredSeasonId);

            var model = new GoalKingViewModel() {
                NoMatchFound = lastMatchsResult == null,
                LastMatchsResult = lastMatchsResult,
                GoalKingRows = goalKingRows
            };
            return View(model);
        }

    }
}
