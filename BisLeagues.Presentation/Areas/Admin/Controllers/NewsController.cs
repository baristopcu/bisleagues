using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.Areas.Admin.BaseControllers;
using BisLeagues.Presentation.Areas.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BisLeagues.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : BaseController<NewsController>
    {
        private readonly INewRepository _newRepository;
        public NewsController(INewRepository newRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _newRepository = newRepository;
        }
        public IActionResult List(FilterNewGetModel filterModel)
        {
            var news = _newRepository.GetGeneralNews();
            if (filterModel.DateFilterStart != default)
                news = news.Where(x => x.CreatedOnUtc.Date > filterModel.DateFilterStart.ToUniversalTime());
            if (filterModel.DateFilterEnd != default)
                news = news.Where(x => x.CreatedOnUtc.Date < filterModel.DateFilterEnd.ToUniversalTime());
            var model = new ListNewViewModel()
            {
                News = news,
                Filters = filterModel
            };
            return View(model);
        }
    }
}