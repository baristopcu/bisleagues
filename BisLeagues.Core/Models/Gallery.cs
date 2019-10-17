using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BisLeagues.Core.Models
{
    public partial class Gallery
    {
        private ICollection<GalleryPhotos> _galleryPhotos;
        private Gallery(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<GalleryPhotos> GalleryPhotos
        {
            get => LazyLoader.Load(this, ref _galleryPhotos);
            set => _galleryPhotos = value;
        }
    }
}
