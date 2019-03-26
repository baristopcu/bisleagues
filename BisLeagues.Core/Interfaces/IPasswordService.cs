using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Interfaces
{
    public interface IPasswordService
    {
        string CreateHash(string password);

        bool VerifyPassword(string password, string goodHash);
    }
}
