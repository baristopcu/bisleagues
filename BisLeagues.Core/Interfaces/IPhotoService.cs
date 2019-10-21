using System.Collections.Generic;
using System.Threading.Tasks;
using BisLeagues.Core.ServiceModels;
using Microsoft.AspNetCore.Http;

namespace BisLeagues.Core.Interfaces
{
    public interface IPhotoService
    {
        Task<string> PlacePhoto(IFormFile photo, string type, int galleryId = 0);
    }
}
