using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Presentation.Areas.Admin.BaseControllers;
using BisLeagues.Presentation.Areas.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BisLeagues.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : BaseController<TeamController>
    {
        private readonly INewRepository _newRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IPhotoService _photoService;

        private readonly ITeamRepository _teamRepository;
        public TeamController(INewRepository newRepository, ISeasonRepository seasonRepository, IPhotoService photoService, ITeamRepository teamRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _newRepository = newRepository;
            _seasonRepository = seasonRepository;
            _photoService = photoService;
            _teamRepository = teamRepository;
        }
        public IActionResult List()
        {
            var teamsWaitingConfirm = _teamRepository.GetTeamsWaitingConfirm().ToList();
            var model = new ListTeamViewModel()
            {
                Teams = teamsWaitingConfirm
            };
            return View(model);
        }
        public IActionResult Confirm(int teamId)
        {
            try
            {
                var team = _teamRepository.Get(teamId);
                team.IsActive = true;
                _teamRepository.Update(team);
                MessageCode = 1;
                Message = "Takım onaylandı !";
                return RedirectToAction("List", "Team");
            }
            catch (Exception)
            {
                MessageCode = 0;
                Message = "Hay aksi ! Onaylayamadım bu takımı..";
                return RedirectToAction("List", "Team");
            }
        }
    }
}