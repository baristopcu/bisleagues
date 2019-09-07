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
    public class ProfileController : BaseController<ProfileController>
    {
        private readonly IUserManager _userManager;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;

        public ProfileController(IUserManager userManager, IPlayerRepository playerRepository, IUserRepository userRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _userManager = userManager;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
        }

        public IActionResult Detail(int id = 0)
        {
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
            return View(user);

        }
    }
}
