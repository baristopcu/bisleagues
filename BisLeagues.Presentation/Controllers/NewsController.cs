using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Presentation.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BisLeagues.Presentation.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewRepository _newRepository;
        private readonly IResultRepository _resultRepository;

        public NewsController(INewRepository newRepository, IResultRepository resultRepository)
        {
            _newRepository = newRepository;
            _resultRepository = resultRepository;
        }

        public IActionResult Detail(int id)
        {
            if (id <= 0)
                return RedirectToAction("Index", "Home");

            var newItem = _newRepository.SingleOrDefault(x => x.Id == id);

            if(newItem == null)
                return RedirectToAction("Index", "Home");

            var result = _resultRepository.SingleOrDefault(x => x.MatchId == newItem.MatchId);

            NewViewModel model = new NewViewModel() {
                New = newItem,
                Result = result
            };

            return View(model);
        }
    }
}