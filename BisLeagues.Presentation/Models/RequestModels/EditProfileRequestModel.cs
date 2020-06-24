using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Models.RequestModels
{
    public class EditProfileRequestModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IFormFile ProfilePicture { get; set; }

    }
}
