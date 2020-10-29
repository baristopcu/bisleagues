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
using BisLeagues.Presentation.BaseControllers;
using Microsoft.Extensions.Caching.Memory;
using BisLeagues.Core.Interfaces;

namespace BisLeagues.Presentation.Controllers
{
    public class MatchController : BaseController<MatchController>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IResultRepository _resultRepository;
        private readonly INewRepository _newRepository;
        private readonly IMemoryCache _memoryCache;

        public MatchController(ISeasonRepository seasonRepository, IMatchRepository matchRepository, IResultRepository resultRepository, INewRepository newRepository, ISettingRepository settingRepository, IMemoryCache memoryCache) : base(settingRepository, memoryCache)
        {
            _seasonRepository = seasonRepository;
            _matchRepository = matchRepository;
            _resultRepository = resultRepository;
            _newRepository = newRepository;
            _memoryCache = memoryCache;

        }

        // GET: Matches
        public IActionResult UpComingMatches(Pagination pagination)
        {
            List<Match> matches = _matchRepository.GetUpcomingMatchesBySeasonId(UserPreferredSeasonId).ToList();
            Match upcomingMatch = _matchRepository.GetUpcomingMatchBySeasonId(UserPreferredSeasonId);
            TimeSpan matchCounter = new TimeSpan();
            if (upcomingMatch != null)
            {
                matchCounter = (upcomingMatch.MatchDate - DateTime.Now);
            }
            UpComingMatchesViewModel model = new UpComingMatchesViewModel()
            {
                NoMatchFound = matches.Count() == 0,
                Matches = matches,
                UpComingMatch = upcomingMatch,
                UpComingMatchCounter = matchCounter
            };
            return View(model);
        }

        // GET: Matches
        public IActionResult PastMatches(Pagination pagination)
        {
            if (pagination == null)
            {
                pagination = new Pagination();
            }
            int totalCount;
            List<New> newsOfPastMatches;
            string newsOfPastMatchesCacheKey = String.Format(MemoryCacheKeys.NewsOfPastMatchesCacheKey, pagination.PageNumber, pagination.PageSize);
            string newsOfPastMatchesTotalCountCacheKey = String.Format(MemoryCacheKeys.NewsOfPastMatchesTotalCountCacheKey, pagination.PageNumber, pagination.PageSize);
            if (_memoryCache.TryGetValue(newsOfPastMatchesCacheKey, out object cachedObject) && _memoryCache.TryGetValue(newsOfPastMatchesTotalCountCacheKey, out object cachedTotalCountObject))
            {
                newsOfPastMatches = (List<New>)cachedObject;
                totalCount = (int)cachedTotalCountObject;
            }
            else
            {
                newsOfPastMatches = _newRepository.GetNewsOfPastMatchesBySeasonId(UserPreferredSeasonId, pagination.GetSkipCount(), pagination.GetPageSize(), out totalCount).ToList();
                _memoryCache.Set(newsOfPastMatchesCacheKey, newsOfPastMatches, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddDays(1)
                });
                _memoryCache.Set(newsOfPastMatchesTotalCountCacheKey, totalCount, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddDays(1)
                });
            }
            pagination.TotalLineCount = totalCount;
            pagination.TotalPageCount = pagination.TotalLineCount % pagination.PageSize == 0 ? (pagination.TotalLineCount / pagination.PageSize) : (pagination.TotalLineCount / pagination.PageSize) + 1;
            Result lastMatchsResult = _resultRepository.GetLastMatchsResultBySeasonId(UserPreferredSeasonId);
            PastMatchesViewModel model = new PastMatchesViewModel()
            {
                Pagination = pagination,
                NoMatchFound = newsOfPastMatches.Count() == 0,
                NewsOfPastMatches = newsOfPastMatches,
                LastMatchsResult = lastMatchsResult
            };
            return View(model);
        }

    }
}
