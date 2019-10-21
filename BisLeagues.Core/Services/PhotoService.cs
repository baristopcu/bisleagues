using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Core.ServiceModels;
using BisLeagues.Core.Services.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IScoreRepository _scoreRepository;

        public PhotoService()
        {
        }

        public async Task<string> PlacePhoto(IFormFile photo, string type, int galleryId = 0)
        {
            bool photoDefined = (photo != null && photo.Length > 0);

            if (photoDefined)
            {
                string extension = Path.GetExtension(photo.FileName);
                if (extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png"))
                {
                    int limit = 2 * 1024 * 1024; //2MB
                    if (photo.Length < limit)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                        string folderPath = "";
                        switch (type)
                        {
                            case "team_logo":
                                folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\team_logos", fileName); break;
                            case "video_pictures":
                                folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\video_pictures", fileName); break;
                            case "news_pictures":
                                folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\news_pictures", fileName); break;
                            case "gallery":
                                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\galleries\\" + galleryId + "\\"));
                                folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\galleries\\"+ galleryId + "\\", fileName); break;
                        }

                        using (var fileSteam = new FileStream(folderPath, FileMode.Create))
                        {
                            await photo.CopyToAsync(fileSteam);
                        }
                        return fileName;

                    }
                    else
                    {
                        return "0"; //size error
                    }

                }
                else
                {
                    return "1"; // format err
                }
            }
            else
            {
                return "2"; // null err
            }
        }

    }
}
