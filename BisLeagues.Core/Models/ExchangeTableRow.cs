﻿using BisLeagues.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BisLeagues.Core.Models
{
    public partial class ExchangeTableRow
    {
        private Season _season;
        private Player _player;

        public ExchangeTableRow()
        {

        }

        private ExchangeTableRow(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        public int Id { get; set; }
        public int SeasonId { get; set; }
        public int PlayerId { get; set; }
        public int Value { get; set; }

        public Season Season
        {
            get => LazyLoader.Load(this, ref _season);
            set => _season = value;
        }
        public Player Player
        {
            get => LazyLoader.Load(this, ref _player);
            set => _player = value;
        }
    }
}
