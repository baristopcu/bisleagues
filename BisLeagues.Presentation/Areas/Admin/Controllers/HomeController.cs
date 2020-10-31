using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.Areas.Admin.BaseControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BisLeagues.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseController<HomeController>
    {
        private readonly IPointTableService _pointTableService;
        private readonly IGoalKingService _goalKingService;
        private readonly IExchangeTableService _exchangeTableService;
        private IMemoryCache _memoryCache;

        public HomeController(ISettingRepository settingRepository,
            IPointTableService pointTableService, IGoalKingService goalKingService,
            IExchangeTableService exchangeTableService,
            IMemoryCache memoryCache) : base(settingRepository)
        {
            _pointTableService = pointTableService;
            _goalKingService = goalKingService;
            _exchangeTableService = exchangeTableService;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ClearCache()
        {
            try
            {
                _memoryCache.Remove(MemoryCacheKeys.HomePageKey);
                _memoryCache.Remove(string.Format(MemoryCacheKeys.GoalKingTableCacheKey, "*", "*"));

                MessageCode = 1;
                Message = "Cache temizlendi :)";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                MessageCode = 0;
                Message = "Cache temizleyemiyorum, bir bilene sorar mısın ?";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> CreateOrUpdatePointTablesForActiveSeasons()
        {
            bool result = await _pointTableService.CreateOrUpdatePointTablesForActiveSeasons();
            if (result)
            {
                MessageCode = 1;
                Message = "Puan tabloları aktif olan sezonlar için güncellendi.";
            }
            else
            {
                MessageCode = 0;
                Message = "Puan tabloları aktif olan sezonlar için güncellenirken hata oluştu !";
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateOrUpdateGoalKingsForActiveSeasons()
        {
            bool result = await _goalKingService.CreateOrUpdateGoalKingTablesForActiveSeasons();
            if (result)
            {
                MessageCode = 1;
                Message = "Gol krallığı tabloları aktif olan sezonlar için güncellendi.";
            }
            else
            {
                MessageCode = 0;
                Message = "Gol krallığı tabloları aktif olan sezonlar için güncellenirken hata oluştu !";
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateOrUpdateExchangeTablesForActiveSeasons()
        {
            bool result = await _exchangeTableService.CreateOrUpdateExchangeTablesForActiveSeasons();
            if (result)
            {
                MessageCode = 1;
                Message = "Borsa tabloları aktif durumda olan sezonlar için güncellendi.";
            }
            else
            {
                MessageCode = 0;
                Message = "Borsa tabloları aktif durumda olan sezonlar için güncellenirken hata oluştu !";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}