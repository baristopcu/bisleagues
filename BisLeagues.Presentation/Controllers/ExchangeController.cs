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
    public class ExchangeController : BaseController<ExchangeController>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IExchangeService _exchangeService;
        private readonly IResultRepository _resultRepository;
        public ExchangeController(ISeasonRepository seasonRepository, IExchangeService exchangeService, IResultRepository resultRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _seasonRepository = seasonRepository;
            _exchangeService = exchangeService;
            _resultRepository = resultRepository;
        }

        public IActionResult Index()
        {
            var lastMatchsResult = _resultRepository.GetLastMatchsResult();
            int activeSeasonId = Request.Cookies["SelectedSeasonId"] != null ? int.Parse(Request.Cookies["SelectedSeasonId"]) : 1;
            List<ExchangeRow> exchangeTableRows = _exchangeService.GetTopPlayersInExchange(activeSeasonId);
            var model = new ExchangeTableViewModel() {
                LastMatchsResult = lastMatchsResult,
                ExchangeTableRows = exchangeTableRows
            };
            return View(model);
        }

    }
}
