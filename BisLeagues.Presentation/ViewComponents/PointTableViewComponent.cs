using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.ServiceModels;
using BisLeagues.Presentation.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BisLeagues.Presentation.ViewComponents
{
    public class PointTableViewComponent : ViewComponent
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IPointTableService _pointTableService;

        public PointTableViewComponent(ISeasonRepository seasonRepository, IPointTableService pointTableService)
        {
            _seasonRepository = seasonRepository;
            _pointTableService = pointTableService;
        }

        public IViewComponentResult Invoke(int numberOfItems)
        {
            int activeSeasonId = Request.Cookies["SelectedSeasonId"] != null ? int.Parse(Request.Cookies["SelectedSeasonId"]) : 1;
            List<PointTableRow> pointTableRows =  _pointTableService.GetPointTableBySeasonId(activeSeasonId);
            pointTableRows = pointTableRows.Count > numberOfItems ? pointTableRows.GetRange(0, numberOfItems) : pointTableRows.GetRange(0, pointTableRows.Count);
            return View(pointTableRows);
        }
    }
}
