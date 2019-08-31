using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IPasswordService _passwordService;
        public UserService(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public AuthenticateResult HandleAuthenticate(string username, string password)
        {
            var user = _userRepository.SingleOrDefault(x => x.Username == username);
            if (user != null)
            {
                bool validation = _passwordService.VerifyPassword(password, user.Password);

                if (validation)
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                    };
                    var identity = new ClaimsIdentity(claims, "1");
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, "1");

                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid password.");
                }
            }
            else
            {
                return AuthenticateResult.Fail("Invalid username.");
            }
        }
    }
}
