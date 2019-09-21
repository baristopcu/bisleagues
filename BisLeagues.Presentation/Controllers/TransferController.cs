using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BisLeagues.Core.Enums;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Core.Utility;
using BisLeagues.Presentation.BaseControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BisLeagues.Presentation.Controllers
{
    public class TransferController : BaseController<TransferController>
    {

        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITransferRequestRepository _transferRequestRepository;
        private readonly IUserManager _userManager;

        public TransferController(ITeamRepository teamRepository, IPlayerRepository playerRepository, ITransferRequestRepository transferRequestRepository, IUserManager userManager, ISettingRepository settingRepository) : base(settingRepository)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _transferRequestRepository = transferRequestRepository;
            _userManager = userManager;
        }


        public IActionResult HandleRequest(int requestId, int status = 0, string redirectTo = "")
        {
            if (status == default)
                return RedirectToAction("Index", "Home");

            var request = _transferRequestRepository.Find(x => x.Id == requestId).FirstOrDefault();

            if (request == null)
                return RedirectToAction("Index", "Home");

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");




            var playerToJoin = request.Player;
            var teamToJoin = request.Team;
            var transferStatus = (TransferStatus)status;

            var redirectControllerName = redirectTo;

            if (string.IsNullOrWhiteSpace(redirectControllerName))
                redirectControllerName = "Home";


            bool isAvailableToHandleRequest = false;
            if (request.Type == (int)TransferTypes.PlayerToTeam && transferStatus == TransferStatus.Cancelled) // Sadece isteğin sahibi, isteğini geri çekebilir.
            {
                isAvailableToHandleRequest = _userManager.GetCurrentUser(this.HttpContext)?.Player == request.Player;
            }
            else if (request.Type == (int)TransferTypes.PlayerToTeam && transferStatus != TransferStatus.Cancelled) // Diğer işlemleri sadece kaptan yapabilir.
            {
                isAvailableToHandleRequest = _userManager.GetCurrentUser(this.HttpContext)?.Player.TeamPlayers.FirstOrDefault()?.Team.CaptainPlayer == _userManager.GetCurrentUser(this.HttpContext)?.Player;
            }
            else if (request.Type == (int)TransferTypes.TeamToPlayer && transferStatus == TransferStatus.Cancelled) // Sadece takım kaptanı yaptığı transfer tekliflerini değerlendirebilir.
            {
                isAvailableToHandleRequest = _userManager.GetCurrentUser(this.HttpContext)?.Player.TeamPlayers.FirstOrDefault()?.Team.CaptainPlayer == request.Team.CaptainPlayer;
            }
            else if (request.Type == (int)TransferTypes.TeamToPlayer && transferStatus != TransferStatus.Cancelled) // Sadece takım kaptanı yaptığı transfer tekliflerini değerlendirebilir.
            {
                isAvailableToHandleRequest = _userManager.GetCurrentUser(this.HttpContext)?.Player == request.Player;
            }

            if (!isAvailableToHandleRequest)
            {
                MessageCode = 0;
                Message = "İsteği onaylamak için uygun değilsiniz !";
                return RedirectToAction("Detail", redirectControllerName);
            }

            if (playerToJoin.TeamPlayers.Count > 0)
            {
                MessageCode = 0;
                Message = "Oyuncu malesef başka bir takıma katılmış bile :(";
                return RedirectToAction("Detail", redirectControllerName);
            }

            switch ((TransferStatus)request.Status)
            {
                case TransferStatus.Rejected:
                    MessageCode = 0;
                    Message = "İstek daha önceden reddedilmiş.";
                    return RedirectToAction("Detail", redirectControllerName);
                case TransferStatus.Confirmed:
                    MessageCode = 0;
                    Message = "İstek daha önceden zaten kabul edilmiş.";
                    return RedirectToAction("Detail", redirectControllerName);
                case TransferStatus.Cancelled:
                    MessageCode = 0;
                    Message = "İstek gönderen tarafından iptal edilmiş :( ";
                    return RedirectToAction("Detail", redirectControllerName);
                case TransferStatus.Pending:
                    if (transferStatus == TransferStatus.Confirmed)
                    {
                        playerToJoin.TeamPlayers.Add(new TeamPlayers() { Player = playerToJoin, Team = teamToJoin });
                        _playerRepository.Update(playerToJoin);
                        request.Status = (int)TransferStatus.Confirmed;
                        _transferRequestRepository.Update(request);
                        MessageCode = 1;
                        Message = "Güzel transfer ! KAP'a bildirdik.";
                    }
                    else if (transferStatus == TransferStatus.Rejected)
                    {
                        request.Status = (int)TransferStatus.Rejected;
                        _transferRequestRepository.Update(request);
                        MessageCode = 2;
                        Message = "Transfer iptal oldu ! Taraftar şaşkın.";
                    }
                    else if (transferStatus == TransferStatus.Cancelled)
                    {
                        request.Status = (int)TransferStatus.Cancelled;
                        _transferRequestRepository.Update(request);
                        MessageCode = 2;
                        Message = "Transfer iptal oldu ! Taraftar şaşkın.";
                    }
                    return RedirectToAction("Detail", redirectControllerName);
            }

            return RedirectToAction("Index", "Home");
        }


        public IActionResult LeaveTeam(int teamId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (teamId == default)
                    return RedirectToAction("Index", "Home");

                var team = _teamRepository.Find(x => x.Id == teamId).FirstOrDefault();
                if (team == null)
                {
                    MessageCode = 0;
                    Message = "Böyle bir takım yok ! Hiç olmadı ki";
                    return RedirectToAction("Index", "Home");
                }

                var player = _userManager.GetCurrentUser(this.HttpContext)?.Player;
                if (player == null)
                {
                    MessageCode = 0;
                    Message = "Böyle bir oyuncu yok ! Hiç olmadı ki";
                    return RedirectToAction("Index", "Home");
                }
                if (player.TeamPlayers.FirstOrDefault()?.Team != team)
                {
                    MessageCode = 0;
                    Message = "Zaten bu takımda değilsin";
                    return RedirectToAction("Detail", "Teams", new { team.Id });
                }

                if (team.CaptainPlayer == player)
                {
                    MessageCode = 0;
                    Message = "Hoop, kaptanlar yarı yolda bırakmaz !";
                    return RedirectToAction("Detail", "Teams", new { team.Id });
                }

                player.TeamPlayers.Remove(player.TeamPlayers.First());
                _playerRepository.Update(player);
                MessageCode = 1;
                Message = "Artık serbestsin !";
                return RedirectToAction("Detail", "Teams", new { team.Id });

            }
            else
            {
                MessageCode = 0;
                Message = "Önce bir giriş yap bakalım !";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult CreateRequest(int teamId = default, int playerId = default, int type = default)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (teamId == default || playerId == default || type == default)
                    return RedirectToAction("Index", "Home");

                var team = _teamRepository.Find(x => x.Id == teamId).FirstOrDefault();
                if (team == null)
                {
                    MessageCode = 0;
                    Message = "Böyle bir takım yok ! Hiç olmadı ki";
                    return RedirectToAction("Index", "Home");
                }

                var player = _playerRepository.Find(x => x.Id == playerId).FirstOrDefault();
                if (player == null)
                {
                    MessageCode = 0;
                    Message = "Böyle bir oyuncu yok ! Hiç olmadı ki";
                    return RedirectToAction("Index", "Home");
                }

                var requestType = (TransferTypes)type;

                if (requestType == TransferTypes.TeamToPlayer)
                {
                    if (team.CaptainPlayer != _userManager.GetCurrentUser(this.HttpContext)?.Player)
                    {
                        MessageCode = 0;
                        Message = "Kaptan olmadan, takıma oyuncu davet edemezsin !";
                        return RedirectToAction("Detail", "Player", new { player.Id });
                    }

                }

                if (player.TeamPlayers.FirstOrDefault() != null)
                {
                    MessageCode = 0;
                    Message = "Oyuncu serbest değil ! Bir takımda oyuncu !";
                    if (requestType == TransferTypes.PlayerToTeam)
                    {
                        return RedirectToAction("Detail", "Teams", new { team.Id });

                    }
                    else
                    {
                        return RedirectToAction("Detail", "Player", new { player.Id });
                    }
                }
                else
                {
                    _transferRequestRepository.Add(new TransferRequest()
                    {
                        Team = team,
                        Player = player,
                        Status = (int)TransferStatus.Pending,
                        Type = type

                    });
                    MessageCode = 1;
                    Message = "Teklif iletildi, bakalım ne olacak !";
                    if (requestType == TransferTypes.PlayerToTeam)
                    {
                        return RedirectToAction("Detail", "Teams", new { team.Id });

                    }
                    else
                    {
                        return RedirectToAction("Detail", "Player", new { player.Id });
                    }

                }

            }
            else
            {
                MessageCode = 0;
                Message = "Önce bir giriş yap bakalım !";
                return RedirectToAction("Index", "Home");
            }
        }

    }
}