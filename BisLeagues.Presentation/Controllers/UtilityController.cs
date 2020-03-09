using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Presentation.BaseControllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BisLeagues.Presentation.Controllers
{
    public class UtilityController : BaseController<UtilityController>
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICountyRepository _countyRepository;
        private readonly ISeasonRepository _seasonRepository;

        public UtilityController(ICityRepository cityRepository, ICountyRepository countyRepository, ISeasonRepository seasonRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _cityRepository = cityRepository;
            _countyRepository = countyRepository;
            _seasonRepository = seasonRepository;
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


        [HttpPost]
        public JsonResult GetSeasons()
        {
            string selectedSeasonId = Request.Cookies["SelectedSeasonId"];
            selectedSeasonId = selectedSeasonId == null ? "0" : selectedSeasonId;
            List<Season> seasons = new List<Season>();
            List<SelectListItem> slSeasons = new List<SelectListItem>();
            seasons = _seasonRepository.GetActiveSeasons().ToList();
            foreach (var item in seasons)
            {
                slSeasons.Add(new SelectListItem { Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.Name.ToLower()), Value = item.Id.ToString() });
            }
            return Json(new SelectList(slSeasons, "Value", "Text", selectedSeasonId));
        }

        public JsonResult SetSelectedSeason(string selectedSeasonId)
        {
            try
            {
                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.UtcNow.AddDays(7);
                Response.Cookies.Append("SelectedSeasonId", selectedSeasonId, cookie);
                MessageCode = 1;
                Message = "Seçim başarılı..";
                return Json("status: true");
            }
            catch (Exception)
            {
                MessageCode = 0;
                Message = "Lig seçimi başarısız, tekrar dener misin ?";
                return Json("status: false");
            }
        }

        public IActionResult SeasonSelector()
        {
            return View();
        }

    }
}