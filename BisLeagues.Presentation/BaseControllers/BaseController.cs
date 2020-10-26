using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BisLeagues.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BisLeagues.Presentation.BaseControllers
{
    public abstract partial class BaseController<T> : Controller where T : BaseController<T>
    {
       private readonly ISettingRepository _settingRepository;
       private readonly IMemoryCache _memoryCache;

        public BaseController(ISettingRepository settingRepository, IMemoryCache memoryCache)
        {
            _settingRepository = settingRepository;
            _memoryCache = memoryCache;
        }

        /* @MessageCode 
*  0 : Error
*  1 : Success
*  2 : Warning
*  3 : Info
* */

        public int MessageCode
        {
            get { return TempData["MessageCode"] == null ? -1 : Convert.ToInt16(TempData["MessageCode"].ToString()); }
            set { TempData["MessageCode"] = value; }
        }

        /* @Message
         * Message's itself
         * */

        public String Message
        {
            get { return TempData["Message"] == null ? String.Empty : TempData["Message"].ToString(); }
            set { TempData["Message"] = value; }
        }

        public int UserPreferredSeasonId;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string companyNameCacheKey = MemoryCacheKeys.CompanyNameCacheKey;
            string companyAddressCacheKey = MemoryCacheKeys.CompanyAddressCacheKey;
            string companyPhoneCacheKey = MemoryCacheKeys.CompanyPhoneCacheKey;
            string companyEmailCacheKey = MemoryCacheKeys.CompanyEmailCacheKey;

            string objCompanyName = "";
            string objCompanyAddress = "";
            string objCompanyPhone = "";
            string objCompanyEmail = "";
            
            if (_memoryCache.TryGetValue(companyNameCacheKey, out string cachedName) 
                && _memoryCache.TryGetValue(companyAddressCacheKey, out string cachedAddress)
                && _memoryCache.TryGetValue(companyPhoneCacheKey, out string cachedPhone)
                && _memoryCache.TryGetValue(companyEmailCacheKey, out string cachedEmail))
            {
                objCompanyName = cachedName;
                objCompanyAddress = cachedAddress;
                objCompanyPhone = cachedPhone;
                objCompanyEmail = cachedEmail;
            }
            else
            {     
                objCompanyName = _settingRepository.SingleOrDefault(x => x.Name == "companysettings.companyname")?.Value;
                objCompanyAddress = _settingRepository.SingleOrDefault(x => x.Name == "companysettings.companyaddress")?.Value;
                objCompanyPhone = _settingRepository.SingleOrDefault(x => x.Name == "companysettings.companyphone")?.Value;
                objCompanyEmail = _settingRepository.SingleOrDefault(x => x.Name == "companysettings.companyemail")?.Value;

                _memoryCache.Set(companyNameCacheKey, objCompanyName,
                    new MemoryCacheEntryOptions() {AbsoluteExpiration = DateTimeOffset.UtcNow.AddMonths(1)});
                _memoryCache.Set(companyAddressCacheKey, objCompanyAddress,
                    new MemoryCacheEntryOptions() {AbsoluteExpiration = DateTimeOffset.UtcNow.AddMonths(1)});
                _memoryCache.Set(companyPhoneCacheKey, objCompanyPhone,
                    new MemoryCacheEntryOptions() {AbsoluteExpiration = DateTimeOffset.UtcNow.AddMonths(1)});
                _memoryCache.Set(companyEmailCacheKey, objCompanyEmail,
                    new MemoryCacheEntryOptions() {AbsoluteExpiration = DateTimeOffset.UtcNow.AddMonths(1)});
            }
            
            UserPreferredSeasonId = Request.Cookies["SelectedSeasonId"] != null ? int.Parse(Request.Cookies["SelectedSeasonId"]) : 0;
            if (context.Controller is Controller controller)
            {
                controller.ViewBag.CompanyName = objCompanyName;
                controller.ViewBag.CompanyAddress = objCompanyAddress;
                controller.ViewBag.CompanyPhone = objCompanyPhone;
                controller.ViewBag.CompanyEmail = objCompanyEmail;
            }

            // string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            // string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            // if (UserPreferredSeasonId == 0 && actionName != "SeasonSelector" && controllerName != "Utility")
            // {
            //     context.Result = RedirectToAction("SeasonSelector", "Utility");
            //     return;
            // }

            UserPreferredSeasonId = UserPreferredSeasonId == 0 ? 3 : UserPreferredSeasonId;

            base.OnActionExecuting(context);
        }
    }
}
