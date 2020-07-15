using System;
using System.Linq;
using System.Transactions;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Presentation.Areas.Admin.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace BisLeagues.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SeasonController : BaseController<SeasonController>
    {
        private readonly ISeasonRepository _seasonRepository;
        public SeasonController(ISeasonRepository seasonRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _seasonRepository = seasonRepository;
        }
        public IActionResult List()
        {
            var seasons = _seasonRepository.GetAll().OrderByDescending(x => x.Id).ToList();
            return View(seasons);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Season season)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (season != null && !String.IsNullOrWhiteSpace(season.Name))
                    {
                        season.StartDate = DateTime.UtcNow;
                        season.StartDate = DateTime.UtcNow.AddYears(1);
                        _seasonRepository.Add(season);
                    }
                    else
                    {
                        MessageCode = 0;
                        Message = "Boş yer bırakma, zaten iki alan var :/";
                        scope.Dispose();
                        return RedirectToAction("Create", "Season");

                    }

                    scope.Complete();
                    MessageCode = 1;
                    Message = "Her şeyi hallettik gibi, iyi işti !";

                    return RedirectToAction("List", "Season");


                }
                catch (Exception)
                {
                    MessageCode = 0;
                    Message = "Bir şeylerde sıçtık, yaptığın her şey boşa gitti :(";
                    scope.Dispose();
                    return RedirectToAction("List", "Season");

                }
            }
        }


        public IActionResult Edit(int seasonId)
        {
            var season = _seasonRepository.Get(seasonId);
            if (season != null)
            {
                return View(season);

            }
            else
            {
                MessageCode = 0;
                Message = "Böyle bir sezon hiç olmadı ki.. Başka bir sezon düzenlemeyi dene :)";
                return RedirectToAction("List", "Season");
            }
        }


        [HttpPost]
        public IActionResult Edit(Season season)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (season != null && !String.IsNullOrWhiteSpace(season.Name))
                    {
                        season.StartDate = DateTime.UtcNow;
                        season.StartDate = DateTime.UtcNow.AddYears(1);
                        _seasonRepository.Update(season);
                    }
                    else
                    {
                        MessageCode = 0;
                        Message = "Boş yer bırakma, zaten iki alan var :/";
                        scope.Dispose();
                        return RedirectToAction("Edit", "Season", new { seasonId = season.Id});

                    }

                    scope.Complete();
                    MessageCode = 1;
                    Message = "Her şeyi hallettik gibi, iyi işti !";

                    return RedirectToAction("List", "Season");


                }
                catch (Exception)
                {
                    MessageCode = 0;
                    Message = "Bir şeylerde sıçtık, yaptığın her şey boşa gitti :(";
                    scope.Dispose();
                    return RedirectToAction("List", "Season");

                }
            }
        }

    }
}