using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BisLeagues.Core.Utility
{
    public class UserManager : IUserManager
    {
        private IUserRepository _userRepository;
        private IPasswordService _passwordService;
        public UserManager(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public User Validate(string username, string password)
        {

            var user = _userRepository.SingleOrDefault(x => x.Username == username);
            if (user != null)
            {
                bool validation = _passwordService.VerifyPassword(password, user.Password);

                if (validation)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async void SignIn(HttpContext httpContext, User user, bool isPersistent = false)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

            var principal = new ClaimsPrincipal(identity);

            var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true,
            };

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal), authProperties);


        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public int GetCurrentUserId(HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return -1;

            Claim claim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
                return -1;

            int currentUserId;

            if (!int.TryParse(claim.Value, out currentUserId))
                return -1;

            return currentUserId;
        }

        public User GetCurrentUser(HttpContext httpContext)
        {
            int currentUserId = this.GetCurrentUserId(httpContext);

            if (currentUserId == -1)
                return null;

            return _userRepository.SingleOrDefault(x => x.Id == currentUserId);
        }

        private IEnumerable<Claim> GetUserClaims(User user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.AddRange(this.GetUserRoleClaims(user));
            return claims;
        }

        private IEnumerable<Claim> GetUserRoleClaims(User user)
        {
            List<Claim> claims = new List<Claim>();
            IEnumerable<UserRole> roles = user.UsersRoles.Select(x => x.UserRole);

            if (roles != null)
            {
                foreach (var role in roles)
                {

                    claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                }
            }

            return claims;
        }
    }
}
