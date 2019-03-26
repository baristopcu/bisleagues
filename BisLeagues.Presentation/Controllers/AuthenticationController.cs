using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BisLeagues.Presentation.Models;
using BisLeagues.Core.Models;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.BaseControllers;
using BisLeagues.Presentation.ViewModels;
using BisLeagues.Core.Interfaces;
using System.Security.Claims;
using BisLeagues.Core.Utility;

namespace BisLeagues.Presentation.Controllers
{
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IUserRepository userRepository, IUserManager userManager) //: base(playerRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;

        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            User user = _userManager.Validate(username, password);

            if (user != null)
                _userManager.SignIn(this.HttpContext, user, false);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            _userManager.SignOut(this.HttpContext);

            return RedirectToAction("Index", "Home");
        }

        //    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //    public IActionResult Error()
        //    {
        //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    }
    }
}
