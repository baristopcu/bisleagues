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
        private readonly IPointTableService _pointTableService;
        private readonly IResultRepository _resultRepository;
        public PointTableController(ISeasonRepository seasonRepository, IPointTableService pointTableService, IResultRepository resultRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _seasonRepository = seasonRepository;
            _pointTableService = pointTableService;
            _resultRepository = resultRepository;
        }

        public IActionResult Index()
        {

            var lastMatchsResult = _resultRepository.GetLastMatchsResult();

            int activeSeasonId = _seasonRepository.GetActiveSeasonId();
            List<PointTableRow> pointTableRows = _pointTableService.GetPointTableBySeasonId(activeSeasonId);
            pointTableRows = pointTableRows.GetRange(0, 6);

            var model = new PointTableViewModel() {
                LastMatchsResult = lastMatchsResult,
                PointTableRows = pointTableRows
            };
            return View(model);
        }

    }
}
