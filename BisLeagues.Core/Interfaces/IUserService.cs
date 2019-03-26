using Microsoft.AspNetCore.Authentication;


namespace BisLeagues.Core.Interfaces
{
    public interface IUserService
    {
        AuthenticateResult HandleAuthenticate(string username, string password);
    }
}
