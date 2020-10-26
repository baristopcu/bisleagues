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
        private readonly IMemoryCache _memoryCache;

        public HomeController(ISettingRepository settingRepository,
            IPointTableService pointTableService, IGoalKingService goalKingService,
            IMemoryCache memoryCache) : base(settingRepository)
        {
            _pointTableService = pointTableService;
            _goalKingService = goalKingService;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ClearCache()
        {
            try
            {
                _memoryCache.Remove(MemoryCacheKeys.HomePageKey);
                // _memoryCache.Remove(MemoryCacheKeys.CompanyNameCacheKey);
                // _memoryCache.Remove(MemoryCacheKeys.CompanyAddressCacheKey);
                // _memoryCache.Remove(MemoryCacheKeys.CompanyPhoneCacheKey);
                // _memoryCache.Remove(MemoryCacheKeys.CompanyEmailCacheKey);

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

        public async Task<IActionResult> UpdatePointTable()
        {
            bool result = await _pointTableService.CreatePointTablesForActiveSeasons();
            if (result)
            {
                MessageCode = 1;
                Message = "Puan tabloları güncellendi.";
            }
            else
            {
                MessageCode = 0;
                Message = "Puan tabloları güncellenirken hata oluştu !";
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UpdateGoalKings()
        {
            bool result = await _goalKingService.CreateGoalKingTablesForActiveSeasons();
            if (result)
            {
                MessageCode = 1;
                Message = "Gol krallığı tabloları güncellendi.";
            }
            else
            {
                MessageCode = 0;
                Message = "Gol krallığu tabloları güncellenirken hata oluştu !";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}