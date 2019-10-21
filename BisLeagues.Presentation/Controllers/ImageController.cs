using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BisLeagues.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace BisLeagues.Presentation.Controllers
{
    public class ImageController : Controller
    {

        private readonly IFileProvider _fileProvider;
        private readonly IDistributedCache _cache;

        public ImageController(IHostingEnvironment env, IDistributedCache cache)
        {
            _fileProvider = env.WebRootFileProvider;
            _cache = cache;
        }



        [Route("/images/{width}/{height}/{*url}")]
        public async Task<IActionResult> Resize(string url, int width = 0, int height = 0)
        {

            if (width < 0 || height < 0) { return BadRequest(); }
            var key = $"/{width}/{height}/{url}";
            var data = await _cache.GetAsync(key);
            if (data == null)
            {
                var imagePath = PathString.FromUriComponent("/" + url);
                var fileInfo = _fileProvider.GetFileInfo(imagePath);
                if (!fileInfo.Exists) { return NotFound(); }
                using (var outputStream = new MemoryStream())
                {
                    using (var inputStream = fileInfo.CreateReadStream())
                    using (Image<Rgba32> image = Image.Load(inputStream))
                    {
                        if (width != 1903 && height != 1903)
                        {
                            image.Mutate(ctx => ctx.Resize(width, height));
                        }
                        image.SaveAsJpeg(outputStream);
                    }
                    data = outputStream.ToArray();
                }
                await _cache.SetAsync(key, data, new DistributedCacheEntryOptions());

            }
            return File(data, "image/jpg");
        }

    }
}