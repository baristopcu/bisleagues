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
    public class PointTableController : BaseController<PointTableController>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IPointTableRowRepository _pointTableRowRepository;
        private readonly IResultRepository _resultRepository;
        public PointTableController(ISeasonRepository seasonRepository, IPointTableRowRepository pointTableRowRepository, IResultRepository resultRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _seasonRepository = seasonRepository;
            _pointTableRowRepository = pointTableRowRepository;
            _resultRepository = resultRepository;
        }

        public IActionResult Index()
        {
            var lastMatchsResult = _resultRepository.GetLastMatchsResultBySeasonId(UserPreferredSeasonId);
            var pointTableRows = _pointTableRowRepository.GetPointTableRowsBySeasonId(UserPreferredSeasonId).ToList();

            var model = new PointTableViewModel() {
                NoMatchFound = lastMatchsResult == null,
                LastMatchsResult = lastMatchsResult,
                PointTableRows = pointTableRows
            };
            return View(model);
        }

    }
}
