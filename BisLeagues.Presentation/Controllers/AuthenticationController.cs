using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BisLeagues.Core.Models;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.BaseControllers;
using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Utility;
using BisLeagues.Presentation.Models.RequestModels;
using Microsoft.Extensions.Caching.Memory;

namespace BisLeagues.Presentation.Controllers
{
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPasswordService _passwordService;

        public AuthenticationController(IUserRepository userRepository, IUserManager userManager, IUserRoleRepository userRoleRepository, IPlayerRepository playerRepository, IPasswordService passwordService, ISettingRepository settingRepository, IMemoryCache memoryCache) : base(settingRepository, memoryCache)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _playerRepository = playerRepository;
            _passwordService = passwordService;
        }


        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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

            return RedirectToAction("Login", "Authentication");
        }


        [HttpGet]
        public IActionResult SignOut()
        {
            _userManager.SignOut(this.HttpContext);
            MessageCode = 1;
            Message = "Çıktın gittin ama hayırlısı.";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult SignUp()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult SignUp(SignUpRequestModel requestModel)
        {
            //TODO's:
            /*
             * Null checks
             * Username unique - Done
             * Email unique - Done
             * Password at least 8 character
             * Role null check
             * Error handling (try catch)
             * 
             */
            requestModel.Username = requestModel.Email;
           var dbUser = _userRepository.Find(x => x.Email == requestModel.Email).FirstOrDefault();
            if (dbUser != null)
            {
                MessageCode = 0;
                Message = "Bu emailden bende zaten var dostum. Başka bir şey dene.";
                return RedirectToAction("SignUp", "Authentication");
            }
            dbUser = _userRepository.Find(x => x.Username == requestModel.Username).FirstOrDefault();
            if (dbUser != null)
            {
                MessageCode = 0;
                Message = "Bu kullanıcı adından bende zaten var dostum. Başka bir isim seç.";
                return RedirectToAction("SignUp", "Authentication");
            }
            if (requestModel.Password.Length < 8)
            {
                MessageCode = 0;
                Message = "Bu şifre çok kolay oldu. 8 karakterden fazla olsun !";
                return RedirectToAction("SignUp", "Authentication");
            }

            string hashedPassword = _passwordService.CreateHash(requestModel.Password);
            User user = new User()
            {
                ProfilePictureId = 3067, //TODO: Take that from settings.
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                Username = requestModel.Email,
                Password = hashedPassword,
                Email = requestModel.Email,
                CreatedOnUtc = DateTime.UtcNow,
            };
            user.Username = user.Email; // be sure, just in case
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
            return RedirectToAction("SignUp", "Authentication");
        }

        //    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //    public IActionResult Error()
        //    {
        //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    }
    }
}
