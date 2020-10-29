using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.Models.ViewModels;
using BisLeagues.Presentation.BaseControllers;
using Microsoft.Extensions.Caching.Memory;

namespace BisLeagues.Presentation.Controllers
{
    public class SearchController : BaseController<SearchController>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;

        public SearchController(ITeamRepository teamRepository, IPlayerRepository playerRepository, ISettingRepository settingRepository, IMemoryCache memoryCache) : base(settingRepository, memoryCache)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;

        }

        public IActionResult Find(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                if (query.Length < 3)
                {
                    MessageCode = 2;
                    Message = "3 harfle aramaya kalkarsak çok uğraşırız, daha fazla detay ver bence :)";
                }
                var teams = _teamRepository.Find(x => x.Name.Contains(query)).ToList();
                var players = _playerRepository.Find(x => (x.User.FirstName + x.User.LastName).Contains(query)).ToList();
                SearchViewModel model = new SearchViewModel()
                {
                    Teams = teams,
                    Players = players
                };
                return View(model);
            }
            else
            {
                MessageCode = 3;
                Message = "Ula, hiçbir şey yazmadın ki ! Neyi arayayım ?";
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
