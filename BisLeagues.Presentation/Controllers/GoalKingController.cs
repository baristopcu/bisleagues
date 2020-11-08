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
using Microsoft.Extensions.Caching.Memory;

namespace BisLeagues.Presentation.Controllers
{
    public class GoalKingController : BaseController<GoalKingController>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IGoalKingRowRepository _goalKingRowRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IMemoryCache _memoryCache;
        public GoalKingController(ISeasonRepository seasonRepository, IGoalKingRowRepository goalKingRowRepository, IResultRepository resultRepository, ISettingRepository settingRepository, IMemoryCache memoryCache) : base(settingRepository, memoryCache)
        {
            _seasonRepository = seasonRepository;
            _goalKingRowRepository = goalKingRowRepository;
            _resultRepository = resultRepository;
            _memoryCache = memoryCache;
        }

        public IActionResult Index(Pagination pagination)
        {
            if (pagination == null)
            {
                pagination = new Pagination();
            }
            int totalCount;
            List<GoalKingRow> goalKingRows;
            string goalKingTableCacheKey = String.Format(MemoryCacheKeys.GoalKingTableCacheKey, pagination.PageNumber, pagination.PageSize);
            string goalKingTableTotalCountCacheKey = String.Format(MemoryCacheKeys.GoalKingTableTotalCountCacheKey, pagination.PageNumber, pagination.PageSize);
            if (_memoryCache.TryGetValue(goalKingTableCacheKey, out object cachedObject) && _memoryCache.TryGetValue(goalKingTableTotalCountCacheKey, out object cachedTotalCountObject))
            {
                goalKingRows = (List<GoalKingRow>)cachedObject;
                totalCount = (int) cachedTotalCountObject;
            }
            else
            {
                goalKingRows = _goalKingRowRepository.GetGoalKingTableRowsBySeasonId(UserPreferredSeasonId, pagination.GetSkipCount(), pagination.GetPageSize(), out totalCount).ToList();
                _memoryCache.Set(goalKingTableCacheKey, goalKingRows, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddDays(1)
                });
                _memoryCache.Set(goalKingTableTotalCountCacheKey, totalCount, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddDays(1)
                });
            }
            pagination.TotalLineCount = totalCount;
            pagination.TotalPageCount = pagination.TotalLineCount % pagination.PageSize == 0 ? (pagination.TotalLineCount / pagination.PageSize) : (pagination.TotalLineCount / pagination.PageSize) + 1;
            var lastMatchsResult = _resultRepository.GetLastMatchsResultBySeasonId(UserPreferredSeasonId);

            var model = new GoalKingViewModel()
            {
                Pagination = pagination,
                NoMatchFound = lastMatchsResult == null,
                LastMatchsResult = lastMatchsResult,
                GoalKingRows = goalKingRows
            };
            return View(model);
        }

    }
}
