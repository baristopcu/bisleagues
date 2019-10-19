using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Presentation.Areas.Admin.BaseControllers;
using BisLeagues.Presentation.Areas.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace BisLeagues.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MatchController : BaseController<MatchController>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IScoreRepository _scoreRepository;
        private readonly INewRepository _newRepository;
        public MatchController(ISeasonRepository seasonRepository, ITeamRepository teamRepository, IMatchRepository matchRepository, IResultRepository resultRepository, IScoreRepository scoreRepository, INewRepository newRepository, ISettingRepository settingRepository) : base(settingRepository)
        {
            _seasonRepository = seasonRepository;
            _teamRepository = teamRepository;
            _matchRepository = matchRepository;
            _resultRepository = resultRepository;
            _scoreRepository = scoreRepository;
            _newRepository = newRepository;
        }
        public IActionResult List(FilterMatchGetModel filterModel)
        {
            var seasonId = _seasonRepository.GetActiveSeasonId();
            var teams = _teamRepository.GetAll().Where(x => x.IsActive = true);
            var matches = _matchRepository.GetMatchesBySeasonId(seasonId);

            if (filterModel.IsPlayed != default)
            {
                var isPlayed = filterModel.IsPlayed == 1 ? true : false;
                matches = matches.Where(x => x.IsPlayed == isPlayed);

            }
            if (filterModel.MatchDateFilterStart != default)
                matches = matches.Where(x => x.MatchDate.Date > filterModel.MatchDateFilterStart.ToUniversalTime());
            if (filterModel.MatchDateFilterEnd != default)
                matches = matches.Where(x => x.MatchDate.Date < filterModel.MatchDateFilterEnd.ToUniversalTime());
            if (filterModel.TeamId != default)
            {
                var team = _teamRepository.Get(filterModel.TeamId);
                matches = matches.Where(x => x.Home == team || x.Away == team);

            }
            matches = matches.OrderByDescending(x => x.Id);
            ListMatchViewModel model = new ListMatchViewModel()
            {
                Teams = teams,
                Matches = matches,
                Filters = filterModel
            };
            return View(model);
        }
        public IActionResult Create()
        {
            var season = _seasonRepository.GetActiveSeason();
            var teams = _teamRepository.GetAll().Where(x => x.IsActive == true);
            CreateMatchViewModel model = new CreateMatchViewModel()
            {
                Season = season,
                Teams = teams
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(CreateMatchPostModel model)
        {
            if (ModelState.IsValid)
            {
                var season = _seasonRepository.GetActiveSeason();
                var home = _teamRepository.Get(model.HomeId);
                var away = _teamRepository.Get(model.AwayId);
                var matchDate = model.MatchDate.AddHours(model.MatchHour.Hours).AddMinutes(model.MatchHour.Minutes).ToUniversalTime();
                var match = new Match()
                {
                    Season = season,
                    Home = home,
                    Away = away,
                    IsPlayed = false,
                    MatchDate = matchDate
                };
                _matchRepository.Add(match);
                MessageCode = 1;
                Message = "Tamam kaydettim.";
                return RedirectToAction("Create", "Match");
            }
            else
            {
                MessageCode = 0;
                Message = "Formda eksiklikler görüyorum, görmeyeyim.";
                return RedirectToAction("Create", "Match");

            }
        }

        public IActionResult Edit(int id)
        {
            if (id != default)
            {
                var season = _seasonRepository.GetActiveSeason();
                var teams = _teamRepository.GetAll().Where(x => x.IsActive == true);
                var match = _matchRepository.Get(id);
                EditMatchViewModel model = new EditMatchViewModel()
                {
                    Season = season,
                    Teams = teams,
                    Match = match
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("List");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMatchPostModel model)
        {
            int matchId = model.MatchId;
            if (matchId != default)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var match = _matchRepository.Get(matchId);
                        if (match != null)
                        {
                            bool needRefresh = false;
                            if (model.MatchDate != default && model.MatchHour != default)
                            {
                                match.MatchDate = model.MatchDate.AddHours(model.MatchHour.Hours).AddMinutes(model.MatchHour.Minutes).ToUniversalTime();
                            }
                            if (model.HomeId != match.HomeId)
                            {
                                var newHomeTeam = _teamRepository.Get(model.HomeId);
                                match.Home = newHomeTeam;
                                needRefresh = true;
                            }
                            if (model.AwayId != match.AwayId)
                            {
                                var newAwayTeam = _teamRepository.Get(model.AwayId);
                                match.Away = newAwayTeam;
                                needRefresh = true;
                            }
                            if (needRefresh)
                            {
                                _matchRepository.Update(match);
                                scope.Complete();
                                MessageCode = 1;
                                Message = "Maçın takımları düzenlendi. Takımlar düzenlendiği için, haber ve diğer detaylar kaydedilmedi. Şu an tekrar kaydedebilirsiniz.";
                                return RedirectToAction("Edit", "Match", new { match.Id });
                            }
                            else
                            {
                                int homeScore = model.HomeScorersIds != null ? model.HomeScorersIds.Count : 0;
                                int awayScore = model.AwayScorersIds != null ? model.AwayScorersIds.Count : 0;

                                var result = _resultRepository.Find(x => x.MatchId == matchId).FirstOrDefault();

                                if (result == null)
                                {
                                    result = new Result()
                                    {
                                        MatchId = matchId,
                                        PlayerOfTheMatchId = model.PlayerOfTheMatchId != default ? model.PlayerOfTheMatchId : 1,
                                        HomeScore = homeScore,
                                        AwayScore = awayScore,
                                    };
                                    _resultRepository.Add(result);
                                }
                                else
                                {
                                    result.PlayerOfTheMatchId = model.PlayerOfTheMatchId != default ? model.PlayerOfTheMatchId : 1;
                                    result.HomeScore = homeScore;
                                    result.AwayScore = awayScore;
                                    _resultRepository.Update(result);
                                }


                                if (result.Id != default)
                                {
                                    var scoreList = new List<Score>();
                                    var oldScores = _scoreRepository.Find(x => x.ResultId == result.Id);
                                    _scoreRepository.RemoveRange(oldScores);
                                    if (model.HomeScorersIds != null && model.HomeScorersIds.Count > 0)
                                    {
                                        foreach (int playerId in model.HomeScorersIds.Select(x => x.Id).Distinct())
                                        {
                                            var score = new Score()
                                            {
                                                ResultId = result.Id,
                                                PlayerId = playerId,
                                                ScoredTeamId = match.HomeId,
                                                Goals = model.HomeScorersIds.Where(x => x.Id == playerId).Count(),
                                                Assists = 0
                                            };
                                            scoreList.Add(score);
                                        }
                                    }
                                    if (model.AwayScorersIds != null && model.AwayScorersIds.Count > 0)
                                    {
                                        foreach (int playerId in model.AwayScorersIds.Select(x => x.Id).Distinct())
                                        {
                                            var score = new Score()
                                            {
                                                ResultId = result.Id,
                                                PlayerId = playerId,
                                                ScoredTeamId = match.AwayId,
                                                Goals = model.AwayScorersIds.Where(x => x.Id == playerId).Count(),
                                                Assists = 0
                                            };
                                            scoreList.Add(score);
                                        }
                                    }
                                    _scoreRepository.AddRange(scoreList);

                                    match.IsPlayed = true;
                                    _matchRepository.Update(match);

                                    var newsForMatch = _newRepository.Find(x => x.MatchId == matchId).FirstOrDefault();
                                    if (newsForMatch == null)
                                    {
                                        newsForMatch = new New();
                                    }

                                    var videoPicture = model.VideoPicture;
                                    var newsPicture = model.NewsPicture;
                                    bool videoPictureDefined = (videoPicture != null && videoPicture.Length > 0);
                                    bool newsPictureDefined = (newsPicture != null && newsPicture.Length > 0);
                                    if (videoPictureDefined || newsPictureDefined)
                                    {
                                        string extensionForVideoPicture = videoPictureDefined ? Path.GetExtension(videoPicture.FileName) : null;
                                        string extensionForNewsPicture = newsPictureDefined ? Path.GetExtension(newsPicture.FileName) : null;
                                        if ((extensionForVideoPicture.Equals(".jpg") || extensionForVideoPicture.Equals(".jpeg") || extensionForVideoPicture.Equals(".png")) &&
                                            (extensionForNewsPicture.Equals(".jpg") || extensionForNewsPicture.Equals(".jpeg") || extensionForNewsPicture.Equals(".png")))
                                        {
                                            int limit = 2 * 1024 * 1024; //2MB
                                            if (videoPictureDefined)
                                            {
                                                if (videoPicture.Length < limit)
                                                {
                                                    var fileName = Guid.NewGuid() + Path.GetExtension(videoPicture.FileName);
                                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\video_pictures", fileName);
                                                    using (var fileSteam = new FileStream(filePath, FileMode.Create))
                                                    {
                                                        await videoPicture.CopyToAsync(fileSteam);
                                                    }
                                                    newsForMatch.VideoCoverPhoto = new Photo()
                                                    {
                                                        Name = fileName,
                                                        Path = "video_pictures/" + fileName,
                                                        DisplayOrder = 1,
                                                        CreatedOnUtc = DateTime.UtcNow
                                                    };

                                                }
                                                else
                                                {
                                                    Message = "Logo 2MB fazla olamaz dostum.";
                                                    return RedirectToAction();
                                                }
                                            }
                                            if (newsPictureDefined)
                                            {
                                                if (newsPicture.Length < limit)
                                                {
                                                    var fileName = Guid.NewGuid() + Path.GetExtension(newsPicture.FileName);
                                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\news_pictures", fileName);
                                                    using (var fileSteam = new FileStream(filePath, FileMode.Create))
                                                    {
                                                        await newsPicture.CopyToAsync(fileSteam);
                                                    }
                                                    newsForMatch.CoverPhoto = new Photo()
                                                    {
                                                        Name = fileName,
                                                        Path = "news_pictures/" + fileName,
                                                        DisplayOrder = 1,
                                                        CreatedOnUtc = DateTime.UtcNow
                                                    };

                                                }
                                                else
                                                {
                                                    Message = "Logo 2MB fazla olamaz dostum.";
                                                    return RedirectToAction();
                                                }
                                            }

                                        }
                                        else
                                        {
                                            Message = "Sadece \".jpg, .jpeg, .png\" uzantılı fotoğrafları yükleyebilirsin.";
                                            return RedirectToAction();
                                        }
                                    }

                                    if (model.Content != null && model.Caption != null)
                                    {
                                        newsForMatch.Caption = model.Caption;
                                        newsForMatch.Content = model.Content;
                                        newsForMatch.ShortDescription = model.ShortDescription;
                                        newsForMatch.VideoUrl = model.VideoUrl;
                                        newsForMatch.MatchId = matchId;
                                        newsForMatch.SeasonId = match.SeasonId;
                                        newsForMatch.CreatedOnUtc = DateTime.UtcNow;
                                        if (newsForMatch.Id == default)
                                        {
                                            _newRepository.Add(newsForMatch);

                                        }
                                        else
                                        {
                                            _newRepository.Update(newsForMatch);
                                        }
                                    }

                                    scope.Complete();
                                    MessageCode = 1;
                                    Message = "Her şeyi hallettik gibi, iyi işti !";

                                    return RedirectToAction("List", "Match");
                                }

                            }
                        }
                        else
                        {
                            return RedirectToAction("List", "Match");
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageCode = 0;
                        Message = "Bir şeylerde sıçtık, yaptığın her şey boşa gitti :(";
                        scope.Dispose();
                        return RedirectToAction("List", "Match");

                    }
                }
            }
            else
            {
                return RedirectToAction("List", "Match");
            }

            return Content("");
        }
    }
}