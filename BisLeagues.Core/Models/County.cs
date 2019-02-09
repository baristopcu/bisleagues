using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BisLeagues.Core.Models
{
    public partial class County
    {
        private City _City;

        public County()
        {
        }

        private County(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }

        public City City
        {
            get => LazyLoader.Load(this, ref _City);
            set => _City = value;
        }
    }
}
