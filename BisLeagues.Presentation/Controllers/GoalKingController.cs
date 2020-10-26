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
using Microsoft.Extensions.Caching.Memory;

namespace BisLeagues.Presentation.Controllers
{
    public class GoalKingController : BaseController<GoalKingController>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IGoalKingRowRepository _goalKingRowRepository;
        private readonly IResultRepository _resultRepository;
        public GoalKingController(ISeasonRepository seasonRepository, IGoalKingRowRepository goalKingRowRepository, IResultRepository resultRepository, ISettingRepository settingRepository, IMemoryCache memoryCache) : base(settingRepository, memoryCache)
        {
            _seasonRepository = seasonRepository;
            _goalKingRowRepository = goalKingRowRepository;
            _resultRepository = resultRepository;
        }

        public IActionResult Index()
        {
            var lastMatchsResult = _resultRepository.GetLastMatchsResultBySeasonId(UserPreferredSeasonId);
            List<GoalKingRow> goalKingRows = _goalKingRowRepository.GetGoalKingTableRowsBySeasonId(UserPreferredSeasonId).ToList();

            var model = new GoalKingViewModel() {
                NoMatchFound = lastMatchsResult == null,
                LastMatchsResult = lastMatchsResult,
                GoalKingRows = goalKingRows
            };
            return View(model);
        }

    }
}
