using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BisLeagues.Core.Utility
{
    public interface IUserManager
    {

        User Validate(string username, string password);

        void SignIn(HttpContext httpContext, User user, bool isPersistent = false);

        void SignOut(HttpContext httpContext);

        int GetCurrentUserId(HttpContext httpContext);

        User GetCurrentUser(HttpContext httpContext);


    }
}
