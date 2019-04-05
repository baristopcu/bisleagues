using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BisLeagues.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BisLeagues.Presentation.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewRepository _newRepository;

        public NewsController(INewRepository newRepository)
        {
            _newRepository = newRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int Id)
        {
            var newItem = _newRepository.SingleOrDefault(x => x.Id == 2);
            return View(newItem);
        }
    }
}