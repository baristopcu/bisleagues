using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BisLeagues.Core.Models;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.BaseControllers;
using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Utility;
using BisLeagues.Presentation.Models.RequestModels;

namespace BisLeagues.Presentation.Controllers
{
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPasswordService _passwordService;

        public AuthenticationController(IUserRepository userRepository, IUserManager userManager, IUserRoleRepository userRoleRepository, IPlayerRepository playerRepository, IPasswordService passwordService, ISettingRepository settingRepository) : base(settingRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _playerRepository = playerRepository;
            _passwordService = passwordService;
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            User user = _userManager.Validate(username, password);

            if (user != null)
            {
                _userManager.SignIn(this.HttpContext, user, false);
                MessageCode = 1;
                Message = "Ne de güzel giriş yaptın.";
            }
            else
            {
                MessageCode = 0;
                Message = "Bir şeyleri yanlış girdin hangisi olduğunu söyleyemem.";
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult SignOut()
        {
            _userManager.SignOut(this.HttpContext);
            MessageCode = 1;
            Message = "Çıktın gittin ama hayırlısı.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult SignUp(SignUpRequestModel requestModel)
        {
            //TODO's:
            /*
             * Null checks
             * Username unique
             * Email unique
             * Password at least 8 character
             * Role null check
             * Error handling (try catch)
             * 
             */
            string hashedPassword = _passwordService.CreateHash(requestModel.Password);
            User user = new User()
            {
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                Username = requestModel.Username,
                Password = hashedPassword,
                Email = requestModel.Email,
                CreatedOnUtc = DateTime.UtcNow,
            };
            _userRepository.Add(user);
            var registeredRole = _userRoleRepository.Find(x => x.RoleName == "Registered").SingleOrDefault();
            user.UsersRoles.Add(new UsersRoles() { UserRole = registeredRole});
            _userRepository.Update(user);
            var player = new Player()
            {
                User = user,
                BirthDate = requestModel.BirthDate
            };
            _playerRepository.Add(player);

            //if everything okey, sign in user
            _userManager.SignIn(this.HttpContext, user);
            MessageCode = 1;
            Message = "Ne de güzel kayıt yaptın.";
            return RedirectToAction("Index", "Home");
        }

        //    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //    public IActionResult Error()
        //    {
        //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    }
    }
}
