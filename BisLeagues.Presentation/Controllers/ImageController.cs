using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace BisLeagues.Presentation.Controllers
{
    public class ImageController : Controller
    {
        public ImageController(IHostingEnvironment env)
        {
            _fileProvider = env.WebRootFileProvider;
        }


        private readonly IFileProvider _fileProvider;

        [Route("/images/{width}/{height}/{*url}")]
        public IActionResult Resize(string url, int width = 0, int height = 0)
        {

            if (width < 0 || height < 0) { return BadRequest(); }

            var imagePath = PathString.FromUriComponent("/" + url);
            var fileInfo = _fileProvider.GetFileInfo(imagePath);
            if (!fileInfo.Exists) { return NotFound(); }

            var outputStream = new MemoryStream();
            using (var inputStream = fileInfo.CreateReadStream())
            using (Image<Rgba32> image = Image.Load(inputStream))
            {
                image.Mutate(ctx => ctx.Resize(width,height));
                image.SaveAsJpeg(outputStream);
            }

            outputStream.Seek(0, SeekOrigin.Begin);

            return File(outputStream, "image/jpg");
        }

    }
}