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
using BisLeagues.Presentation.Models.RequestModels;
using System.IO;
using Microsoft.AspNetCore.Http;
using BisLeagues.Presentation.BaseControllers;
using BisLeagues.Core.Utility;

namespace BisLeagues.Presentation.Controllers
{
    public class TeamsController : BaseController<TeamsController>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IUserManager _userManager;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;

        public TeamsController(ICityRepository cityRepository, IPhotoRepository photoRepository, IUserManager userManager, ITeamRepository teamRepository, IPlayerRepository playerRepository)
        {
            _cityRepository = cityRepository;
            _photoRepository = photoRepository;
            _userManager = userManager;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
        }

        public IActionResult Application()
        {
            if (User.Identity.IsAuthenticated)
            {
                TeamApplicationViewModel model = new TeamApplicationViewModel
                {
                    Cities = _cityRepository.GetAll().ToList()
                };
                return View(model);
            }
            else
            {
                MessageCode = 0;
                Message = "Takım başvurusu yapmadan önce sisteme giriş yapmalısın.";
                return RedirectToAction("Index", "Home");

            }

        }

        [HttpPost]
        public async Task<IActionResult> Application(TeamApplicationRequestModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                MessageCode = 0;
                Message = "Takım başvurusu yapmadan önce sisteme giriş yapmalısın.";
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var image = model.Logo;
                if (image != null && image.Length > 0)
                {
                    string extension = Path.GetExtension(image.FileName);
                    if (extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png"))
                    {
                        int limit = 2 * 1024 * 1024; //2MB
                        if (image.Length < limit)
                        {
                            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\team_logos", fileName);
                            using (var fileSteam = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(fileSteam);
                            }

                            var team = new Team()
                            {
                                Name = model.TeamName,
                                CityId = model.City,
                                CountyId = model.County,
                                CaptainPlayerId = _userManager.GetCurrentUser(this.HttpContext).Id,
                                Level = 0,
                                IsActive = false,
                                CreatedOnUtc = DateTime.UtcNow
                            };
                            team.Logo = new Photo()
                            {
                                Name = fileName,
                                Path = "team_logos/" + fileName,
                                DisplayOrder = 1,
                                CreatedOnUtc = DateTime.UtcNow
                            };
                            _teamRepository.Add(team);
                            //TODO: Send notification to admin for confirmation.
                        }
                        else
                        {
                            Message = "Logo 2MB fazla olamaz dostum.";
                            return RedirectToAction();
                        }
                    }
                    else
                    {
                        Message = "Sadece \".jpg, .jpeg, .png\" uzantılı fotoğrafları yükleyebilirsin.";
                        return RedirectToAction();
                    }
                }
            }
            else
            {
                Message = "Bir şeyleri yanlış yaptın. Dikkatli doldur.";
                return RedirectToAction();
            }

            //Success
            MessageCode = 1;
            Message = "Güzel takım kayıt başvurunu aldık, biz bir bakıp hemen onay vericez.";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Detail(int id)
        {
            if (id == 0 && User.Identity.IsAuthenticated)
            {
                var player = _userManager.GetCurrentUser(this.HttpContext).Player;
                if (player!=null)
                {
                    id = player.TeamPlayers.FirstOrDefault().TeamId;
                }
            }

            var team = _teamRepository.Find(x => x.Id == id).FirstOrDefault();
            if (team == null)
            {
                MessageCode = 0;
                Message = "Böyle bir takım yok ! Hiç olmadı ki";
                return RedirectToAction("Index", "Home");
            }
            return View(team);

        }
    }
}
