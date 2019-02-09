using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BisLeagues.Core.Models
{
    public partial class City
    {
        private ICollection<County> _Counties;

        public City()
        {
        }

        private City(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<County> Counties
        {
            get => LazyLoader.Load(this, ref _Counties);
            set => _Counties = value;
        }
    }
}
