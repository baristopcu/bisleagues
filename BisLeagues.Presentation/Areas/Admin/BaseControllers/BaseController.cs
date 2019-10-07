using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Areas.Admin.BaseControllers
{
    [Area("Admin")]
    public abstract partial class BaseController<T> : Controller where T : BaseController<T>
    {
        private readonly ISettingRepository _settingRepository;

        public BaseController(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
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

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var objCompanyName = _settingRepository.Find(x => x.Name == "companysettings.companyname").FirstOrDefault();
            var objCompanyAddress = _settingRepository.Find(x => x.Name == "companysettings.companyaddress").FirstOrDefault();
            var objCompanyPhone = _settingRepository.Find(x => x.Name == "companysettings.companyphone").FirstOrDefault();
            var objCompanyEmail = _settingRepository.Find(x => x.Name == "companysettings.companyemail").FirstOrDefault();

            if (context.Controller is Controller controller)
            {
                var userRoles = User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(y => y.Value).ToList();
                if (!userRoles.Contains("1"))
                {
                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"controller", "Home"},
                            {"action", "Index"},
                            {"area", "" }
                        });
                }
                controller.ViewBag.CompanyName = objCompanyName != null ? objCompanyName.Value : "";
                controller.ViewBag.CompanyAddress = objCompanyAddress != null ? objCompanyAddress.Value : "";
                controller.ViewBag.CompanyPhone = objCompanyPhone != null ? objCompanyPhone.Value : "";
                controller.ViewBag.CompanyEmail = objCompanyEmail != null ? objCompanyEmail.Value : "";
            }
            base.OnActionExecuting(context);
        }
    }
}
