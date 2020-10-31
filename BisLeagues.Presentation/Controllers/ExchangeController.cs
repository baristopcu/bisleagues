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
    public class ExchangeController : BaseController<ExchangeController>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IExchangeTableRowRepository _exchangeTableRowRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IMemoryCache _memoryCache;
        public ExchangeController(ISeasonRepository seasonRepository,
            IExchangeTableRowRepository exchangeTableRowRepository, IResultRepository resultRepository, ISettingRepository settingRepository, IMemoryCache memoryCache) : base(settingRepository, memoryCache)
        {
            _seasonRepository = seasonRepository;
            _exchangeTableRowRepository = exchangeTableRowRepository;
            _resultRepository = resultRepository;
            _memoryCache = memoryCache;
        }

        public IActionResult Index(Pagination pagination)
        {
            if (pagination == null)
            {
                pagination = new Pagination();
            }
            var lastMatchsResult = _resultRepository.GetLastMatchsResultBySeasonId(UserPreferredSeasonId);

            List<ExchangeTableRow> exchangeTableRows;
            int totalCount;
            string exchangeTableCacheKey = String.Format(MemoryCacheKeys.ExchangeTableCacheKey, pagination.PageNumber, pagination.PageSize);
            string exchangeTableTotalCountCacheKey = String.Format(MemoryCacheKeys.ExchangeTableTotalCountCacheKey, pagination.PageNumber, pagination.PageSize);
            if (_memoryCache.TryGetValue(exchangeTableCacheKey, out object cachedObject) && _memoryCache.TryGetValue(exchangeTableTotalCountCacheKey, out object cachedTotalCountObject))
            {
                exchangeTableRows = (List<ExchangeTableRow>)cachedObject;
                totalCount = (int) cachedTotalCountObject;
            }
            else
            {
                exchangeTableRows = _exchangeTableRowRepository.GetExchangeTableRowsBySeasonId(UserPreferredSeasonId, pagination.GetSkipCount(), pagination.GetPageSize(), out totalCount).ToList();
                _memoryCache.Set(exchangeTableCacheKey, exchangeTableRows, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddDays(1)
                });
                _memoryCache.Set(exchangeTableTotalCountCacheKey, totalCount, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddDays(1)
                });
            }
            pagination.TotalLineCount = totalCount;
            pagination.TotalPageCount = pagination.TotalLineCount % pagination.PageSize == 0 ? (pagination.TotalLineCount / pagination.PageSize) : (pagination.TotalLineCount / pagination.PageSize) + 1;
            var model = new ExchangeTableViewModel()
            {
                Pagination = pagination,
                NoMatchFound = lastMatchsResult == null,
                LastMatchsResult = lastMatchsResult,
                ExchangeTableRows = exchangeTableRows
            };
            return View(model);
        }

    }
}
