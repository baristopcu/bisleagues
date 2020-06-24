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
    public class ProfileController : BaseController<ProfileController>
    {
        private readonly IUserManager _userManager;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransferRequestRepository _transferRequestRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IGoalKingService _goalKingService;

        public ProfileController(IUserManager userManager, IPlayerRepository playerRepository, IUserRepository userRepository, ITransferRequestRepository transferRequestRepository, ISeasonRepository seasonRepository, IGoalKingService goalKingService, ISettingRepository settingRepository) : base(settingRepository)
        {
            _userManager = userManager;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
            _transferRequestRepository = transferRequestRepository;
            _seasonRepository = seasonRepository;
            _goalKingService = goalKingService;
        }

        public IActionResult Detail(int id = 0)
        {
            if (id == 0 && !User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            User user;
            if (id == 0 && User.Identity.IsAuthenticated)
            {
                user = _userManager.GetCurrentUser(this.HttpContext);
            }
            else
            {
                user = _userRepository.Find(x => x.Id == id).FirstOrDefault();
            }

            if (user == null)
            {
                MessageCode = 0;
                Message = "Böyle bir kullanıcı yok ! Hiç olmadı ki";
                return RedirectToAction("Index", "Home");
            }

            List<TransferRequest> incomingTransferRequests = new List<TransferRequest>();
            List<TransferRequest> outgoingTransferRequests = new List<TransferRequest>();
            if (_userManager.GetCurrentUser(this.HttpContext)?.Player != null && _userManager.GetCurrentUser(this.HttpContext)?.Player == user.Player)
            {
                incomingTransferRequests = _transferRequestRepository.Find(x => x.Type == (int)TransferTypes.TeamToPlayer && x.Player == user.Player).ToList();
                outgoingTransferRequests = _transferRequestRepository.Find(x => x.Type == (int)TransferTypes.PlayerToTeam && x.Player == user.Player).ToList();
            }
            var totalGoalCount = _goalKingService.GetPlayersGoalsByPlayerIdAndSeasonId(user.Player.Id, UserPreferredSeasonId);
            var model = new UserDetailViewModel()
            {
                User = user,
                TotalGoalCount = totalGoalCount,
                IncomingTransferRequests = incomingTransferRequests,
                OutgoingTransferRequests = outgoingTransferRequests,
            };
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileRequestModel model)
        {
            try
            {
                var user = _userManager.GetCurrentUser(this.HttpContext);

                if (user != null)
                {
                    var image = model.ProfilePicture;
                    if (image != null && image.Length > 0)
                    {
                        string extension = Path.GetExtension(image.FileName);
                        if (extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png"))
                        {
                            int limit = 2 * 1024 * 1024; //2MB
                            if (image.Length < limit)
                            {
                                var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/user_pictures", fileName);
                                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileSteam);
                                }
                                user.ProfilePicture = new Photo()
                                {
                                    Name = fileName,
                                    Path = "user_pictures/" + fileName,
                                    DisplayOrder = 1,
                                    CreatedOnUtc = DateTime.UtcNow
                                };
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
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    _userRepository.Update(user);
                    MessageCode = 1;
                    Message = "Her şeyi hallettik, yeni profilin hayırlı olsun.";
                    return RedirectToAction("Detail", "Profile");
                }
                else
                {
                    MessageCode = 0;
                    Message = "Sen aslında yoksun, bir giriş yapsan mı acaba ?";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                MessageCode = 0;
                Message = "Bir şeyler fena ters gitti ama ne gitti inan bende bilmiyorum :(";
                return RedirectToAction("Detail", "Profile");
            }

        }
    }
}
