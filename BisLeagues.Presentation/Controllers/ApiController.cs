using Microsoft.AspNetCore.Mvc;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.Models.ViewModels;
using BisLeagues.Presentation.BaseControllers;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace BisLeagues.Presentation.Controllers
{
    public class ApiController : BaseController<ApiController>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IExchangeTableRowRepository _exchangeTableRowRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IMemoryCache _memoryCache;
        public ApiController(ISeasonRepository seasonRepository,
            IExchangeTableRowRepository exchangeTableRowRepository, IResultRepository resultRepository, ISettingRepository settingRepository, IMemoryCache memoryCache) : base(settingRepository, memoryCache)
        {
            _seasonRepository = seasonRepository;
            _exchangeTableRowRepository = exchangeTableRowRepository;
            _resultRepository = resultRepository;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            var result = _resultRepository.GetResultsOfMatchesForJson();
            return Json(result);
        }

    }
}
