using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Presentation.Areas.Admin.BaseControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BisLeagues.Presentation.Areas.Admin.Controllers
{
    public class UtilityController : BaseController<UtilityController>
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICountyRepository _countyRepository;

        public UtilityController(ICityRepository cityRepository, ICountyRepository countyRepository, ISettingRepository settingRepository) : base (settingRepository)
        {
            _cityRepository = cityRepository;
            _countyRepository = countyRepository;

        }

        [HttpPost]
        public JsonResult GetCounties(string strCityId)
        {
            int cityId = Convert.ToInt32(strCityId);
            List<County> counties = new List<County>();
            List<SelectListItem> slCounties = new List<SelectListItem>();
            counties = _countyRepository.Find(x => x.CityId == cityId).ToList();
            slCounties.Add(new SelectListItem { Text = "Şimdi ilçe seçiniz..", Value = "" });
            foreach (var item in counties)
            {
                slCounties.Add(new SelectListItem { Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.Name.ToLower()), Value = item.Id.ToString() });
            }
            return Json(new SelectList(slCounties, "Value", "Text"));
        }


    }
}