﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Models
{
    public partial class Photo
    {

        private ICollection<GalleryPhotos> _galleryPhotos;
        public Photo()
        {

        }

        private Photo(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedOnUtc { get; set; }

        public ICollection<GalleryPhotos> GalleryPhotos
        {
            get => LazyLoader.Load(this, ref _galleryPhotos);
            set => _galleryPhotos = value;
        }
    }
}
