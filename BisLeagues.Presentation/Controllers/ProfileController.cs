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

namespace BisLeagues.Presentation.Controllers
{
    public class ProfileController : BaseController<ProfileController>
    {
        private readonly IUserManager _userManager;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransferRequestRepository _transferRequestRepository;

        public ProfileController(IUserManager userManager, IPlayerRepository playerRepository, IUserRepository userRepository, ITransferRequestRepository transferRequestRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _userManager = userManager;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
            _transferRequestRepository = transferRequestRepository;
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

            var model = new UserDetailViewModel()
            {
                User = user,
                IncomingTransferRequests = incomingTransferRequests,
                OutgoingTransferRequests = outgoingTransferRequests,
            };
            return View(model);

        }
    }
}
