using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BisLeagues.Presentation.ViewComponents
{
    public class PointTableViewComponent : ViewComponent
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IPointTableRowRepository _pointTableRowRepository;

        public PointTableViewComponent(ISeasonRepository seasonRepository, IPointTableRowRepository pointTableRowRepository)
        {
            _seasonRepository = seasonRepository;
            _pointTableRowRepository = pointTableRowRepository;
        }

        public IViewComponentResult Invoke(int numberOfItems)
        {
            int activeSeasonId = Request.Cookies["SelectedSeasonId"] != null ? int.Parse(Request.Cookies["SelectedSeasonId"]) : 3;
            List<PointTableRow> pointTableRows =  _pointTableRowRepository.GetPointTableRowsBySeasonId(activeSeasonId).ToList();
            pointTableRows = pointTableRows.Count > numberOfItems ? pointTableRows.GetRange(0, numberOfItems) : pointTableRows.GetRange(0, pointTableRows.Count);
            return View(pointTableRows);
        }
    }
}
