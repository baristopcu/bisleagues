using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.Areas.Admin.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace BisLeagues.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MatchController : BaseController<MatchController>
    {
        public MatchController(ISettingRepository settingRepository) : base(settingRepository)
        {

        }
        public IActionResult Create()
        {
            return View();
        }
    }
}