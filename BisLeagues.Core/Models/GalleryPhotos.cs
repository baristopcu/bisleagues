
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BisLeagues.Core.Models
{
    public partial class GalleryPhotos
    {

        private Gallery _gallery;
        private Photo _photo;

        public GalleryPhotos()
        {
        }

        private GalleryPhotos(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }
        [Key]
        public int GalleryId { get; set; }
        [Key]
        public int PhotoId { get; set; }

        [ForeignKey("GalleryId")]
        public Gallery Gallery
        {
            get => LazyLoader.Load(this, ref _gallery);
            set => _gallery = value;
        }
        [ForeignKey("PhotoId")]
        public Photo Photo
        {
            get => LazyLoader.Load(this, ref _photo);
            set => _photo = value;
        }
    }
}
