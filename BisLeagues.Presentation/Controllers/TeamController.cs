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
using BisLeagues.Core.Enums;
using BisLeagues.Core.Interfaces;

namespace BisLeagues.Presentation.Controllers
{
    public class TeamController : BaseController<TeamController>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IUserManager _userManager;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITransferRequestRepository _transferRequestRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IGoalKingService _goalKingService;

        public TeamController(ICityRepository cityRepository, IPhotoRepository photoRepository, IUserManager userManager, ITeamRepository teamRepository, IPlayerRepository playerRepository, ITransferRequestRepository transferRequestRepository, IGoalKingService goalKingService, ISeasonRepository seasonRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _cityRepository = cityRepository;
            _photoRepository = photoRepository;
            _userManager = userManager;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _transferRequestRepository = transferRequestRepository;
            _seasonRepository = seasonRepository;
            _goalKingService = goalKingService;
        }

        public IActionResult Application()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (_userManager.GetCurrentUser(this.HttpContext).Player.TeamPlayers.Count > 0)
                {
                    MessageCode = 0;
                    Message = "Bir takımın varken takım başvurusu oluşturamazsın, önce takımından ayrıl";
                    return RedirectToAction("Index", "Home");
                }
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

                if (_userManager.GetCurrentUser(this.HttpContext).Player.TeamPlayers.Count > 0)
                {
                    MessageCode = 0;
                    Message = "Bir takımın varken takım başvurusu oluşturamazsın, önce takımından ayrıl";
                    return RedirectToAction("Index", "Home");
                }
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
                            var user = _userManager.GetCurrentUser(this.HttpContext);
                            var team = new Team()
                            {
                                Name = model.TeamName,
                                CityId = model.City,
                                CountyId = model.County,
                                CaptainPlayerId = user.Player.Id,
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
                            team.TeamPlayers.Add(new TeamPlayers() { Player = user.Player, Team = team });
                            _teamRepository.Update(team);
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

        public IActionResult Detail(int id = 0)
        {
            if (id == 0 && User.Identity.IsAuthenticated)
            {
                var player = _userManager.GetCurrentUser(this.HttpContext).Player;
                if (player != null)
                {
                    var teamPlayers = player.TeamPlayers.FirstOrDefault();
                    if (teamPlayers != null)
                    {
                        id = teamPlayers.TeamId;
                    }
                }
            }

            var team = _teamRepository.Find(x => x.Id == id).FirstOrDefault();
            if (team == null)
            {
                MessageCode = 0;
                Message = "Böyle bir takım yok ! Hiç olmadı ki";
                return RedirectToAction("Index", "Home");
            }

            List<TransferRequest> incomingTransferRequests = new List<TransferRequest>();
            List<TransferRequest> outgoingTransferRequests = new List<TransferRequest>();
            if (_userManager.GetCurrentUser(this.HttpContext)?.Player == team.CaptainPlayer)
            {
                incomingTransferRequests = _transferRequestRepository.Find(x => x.Type == (int)TransferTypes.PlayerToTeam && x.Team == team).ToList();
                outgoingTransferRequests = _transferRequestRepository.Find(x => x.Type == (int)TransferTypes.TeamToPlayer && x.Team == team).ToList();
            }
            var activeSeasonId = _seasonRepository.GetActiveSeasonId();
            var totalGoalCount = _goalKingService.GetTeamGoalsByTeamIdAndSeasonId(team.Id, activeSeasonId);
            var model = new TeamDetailViewModel()
            {
                Team = team,
                TotalGoalCount = totalGoalCount,
                IncomingTransferRequests = incomingTransferRequests,
                OutgoingTransferRequests = outgoingTransferRequests,
            };
            return View(model);

        }

        [HttpPost]
        public IActionResult EditDescription(int teamId, string description)
        {
            if (User.Identity.IsAuthenticated)
            {
                var player = _userManager.GetCurrentUser(this.HttpContext).Player;
                var team = _teamRepository.Find(x => x.Id == teamId).FirstOrDefault();
                if (team == null)
                {
                    MessageCode = 0;
                    Message = "Böyle bir takım yok ! Hiç olmadı ki";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (player == team.CaptainPlayer)
                    {
                        description = description.Replace("script", "");
                        team.Description = description;
                        _teamRepository.Update(team);
                        MessageCode = 1;
                        Message = "Takım açıklamasını güncelledik.";
                        return RedirectToAction("Detail", "Team", new { id = teamId });
                    }
                    else
                    {
                        MessageCode = 0;
                        Message = "Sen kaptan değilsin, düzenleyemezsin !";
                        return RedirectToAction("Detail", "Team", new { id = teamId });
                    }
                }
            }
            else
            {
                MessageCode = 0;
                Message = "Önce bir giriş yap hele !";
                return RedirectToAction("Detail", "Team", new { id = teamId });
            }
        }
    }
}
