using BisLeagues.Core.Interfaces;
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
        private readonly IPhotoService _photoService;
        private readonly IPointRepository _pointRepository;
        public MatchController(ISeasonRepository seasonRepository, ITeamRepository teamRepository, IMatchRepository matchRepository, IResultRepository resultRepository, IScoreRepository scoreRepository, INewRepository newRepository, IPhotoService photoService, ISettingRepository settingRepository, IPointRepository pointRepository) : base(settingRepository)
        {
            _seasonRepository = seasonRepository;
            _teamRepository = teamRepository;
            _matchRepository = matchRepository;
            _resultRepository = resultRepository;
            _scoreRepository = scoreRepository;
            _newRepository = newRepository;
            _photoService = photoService;
            _pointRepository = pointRepository;
        }
        public IActionResult List(FilterMatchGetModel filterModel)
        {
            var teams = _teamRepository.GetAll().Where(x => x.IsActive = true);
            var seasons = _seasonRepository.GetActiveSeasons();
            var matches = _matchRepository.GetMatches();

            if (filterModel.SeasonId != default)
            {
                matches = matches.Where(x => x.SeasonId == filterModel.SeasonId);

            }
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
                Seasons = seasons,
                Teams = teams,
                Matches = matches,
                Filters = filterModel
            };
            return View(model);
        }
        public IActionResult Create()
        {
            var seasons = _seasonRepository.GetActiveSeasons();
            var teams = _teamRepository.GetAll().Where(x => x.IsActive == true);
            CreateMatchViewModel model = new CreateMatchViewModel()
            {
                Seasons = seasons,
                Teams = teams
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(CreateMatchPostModel model)
        {
            if (ModelState.IsValid)
            {
                var season = _seasonRepository.Get(model.SeasonId);
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
                var seasons = _seasonRepository.GetActiveSeasons().ToList();
                var teams = _teamRepository.GetAll().Where(x => x.IsActive == true);
                var match = _matchRepository.Get(id);
                Point point = null;

                if (match != null && match.Result != null)
                    point = _pointRepository.Find(x => x.ResultId == match.Result.Id).FirstOrDefault();

                if (point == null)
                    point = new Point();

                var playerList = match.Home.TeamPlayers.Select(x => x.Player).ToList();
                playerList.AddRange(match.Away.TeamPlayers.Select(x => x.Player).ToList());
                EditMatchViewModel model = new EditMatchViewModel()
                {
                    Seasons = seasons,
                    Teams = teams,
                    Match = match,
                    Point = point,
                    PlayerList = playerList
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
                                needRefresh = true;
                            }
                            if (model.SeasonId != match.SeasonId)
                            {
                                match.SeasonId = model.SeasonId;
                                needRefresh = true;
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
                                Message = "Maçın takımları veya sezon düzenlendi. Takımlar düzenlendiği için, haber ve diğer detaylar kaydedilmedi. Şu an tekrar kaydedebilirsiniz.";
                                return RedirectToAction("Edit", "Match", new { match.Id });
                            }
                            else
                            {
                                int homeScore = model.HomeScorersIds != null ? model.HomeScorersIds.Where(x => x.Id != default).Count() : 0;
                                int awayScore = model.AwayScorersIds != null ? model.AwayScorersIds.Where(x => x.Id != default).Count() : 0;

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
                                            if (playerId != default)
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
                                    }
                                    if (model.AwayScorersIds != null && model.AwayScorersIds.Count > 0)
                                    {
                                        foreach (int playerId in model.AwayScorersIds.Select(x => x.Id).Distinct())
                                        {

                                            if (playerId != default)
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
                                    }
                                    _scoreRepository.AddRange(scoreList);

                                    var point = _pointRepository.Find(x=>x.Result == result).FirstOrDefault();
                                    if (point == null)
                                    {
                                        point = new Point()
                                        {
                                            Season = match.Season,
                                            Result = result,
                                            HomePoint = model.HomePoint,
                                            AwayPoint = model.AwayPoint,
                                            CreatedOnUtc = DateTime.UtcNow
                                        };

                                        _pointRepository.Add(point);
                                    }
                                    else
                                    {
                                        point.Season = match.Season;
                                        point.Result = result;
                                        point.HomePoint = model.HomePoint;
                                        point.AwayPoint = model.AwayPoint;
                                        point.CreatedOnUtc = DateTime.UtcNow;
                                        _pointRepository.Update(point);
                                    }


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
                                        if (videoPictureDefined)
                                        {
                                            if (newsForMatch.Id != default && newsForMatch.VideoCoverPhotoId != default)
                                            {
                                                var oldPhoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/video_pictures", newsForMatch.VideoCoverPhoto.Name);
                                                System.IO.File.Delete(oldPhoto);
                                            }
                                            string videoPictureFileName = await _photoService.PlacePhoto(videoPicture, "video_pictures");

                                            if (videoPictureFileName == "0" || videoPictureFileName == "1" || videoPictureFileName == "2")
                                            {
                                                MessageCode = 0;
                                                switch (videoPictureFileName)
                                                {
                                                    case "0":
                                                        Message = "Bu çok büyük be, 2MB fazla fotoğraf yüklemeyelim. ";
                                                        break;

                                                    case "1":
                                                        Message = "Sadece fotoğraf kabul ediyoruz dostum. O kadar !";
                                                        break;

                                                    case "2":
                                                        Message = "Fotoğraf boştu ? Ama doluydu da, teknik bir hata var";
                                                        break;
                                                }
                                                return RedirectToAction("Edit", "Match", new { match.Id });
                                            }
                                            else
                                            {

                                                newsForMatch.VideoCoverPhoto = new Photo()
                                                {
                                                    Name = videoPictureFileName,
                                                    Path = "video_pictures/" + videoPictureFileName,
                                                    DisplayOrder = 1,
                                                    CreatedOnUtc = DateTime.UtcNow
                                                };
                                            }
                                        }

                                        if (newsPictureDefined)
                                        {
                                            if (newsForMatch.Id != default && newsForMatch.CoverPhotoId != default)
                                            {
                                                var oldPhoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/news_pictures", newsForMatch.CoverPhoto.Name);
                                                System.IO.File.Delete(oldPhoto);
                                            }
                                            string newsPictureFileName = await _photoService.PlacePhoto(newsPicture, "news_pictures");
                                            if (newsPictureFileName == "0" || newsPictureFileName == "1" || newsPictureFileName == "2")
                                            {
                                                MessageCode = 0;
                                                switch (newsPictureFileName)
                                                {
                                                    case "0":
                                                        Message = "Bu çok büyük be, 2MB fazla fotoğraf yüklemeyelim. ";
                                                        break;

                                                    case "1":
                                                        Message = "Sadece fotoğraf kabul ediyoruz dostum. O kadar !";
                                                        break;

                                                    case "2":
                                                        Message = "Fotoğraf boştu ? Ama doluydu da, teknik bir hata var";
                                                        break;
                                                }
                                                return RedirectToAction("Edit", "Match", new { match.Id });
                                            }
                                            else
                                            {
                                                newsForMatch.CoverPhoto = new Photo()
                                                {
                                                    Name = newsPictureFileName,
                                                    Path = "news_pictures/" + newsPictureFileName,
                                                    DisplayOrder = 1,
                                                    CreatedOnUtc = DateTime.UtcNow
                                                };
                                            }

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

                                        var formGalleryFiles = Request.Form.Files.Where(x => x.Name.Contains("GalleryPhotos")).ToList();
                                        if (formGalleryFiles != null && formGalleryFiles.Count() > 0)
                                        {

                                            var gallery = newsForMatch.Gallery;
                                            if (gallery == null)
                                            {
                                                gallery = new Gallery()
                                                {
                                                    Name = "Galeri"
                                                };
                                                newsForMatch.Gallery = gallery;
                                            }
                                            else
                                            {
                                                //DirectoryInfo info = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\galleries", gallery.Id.ToString()));
                                                //if (info.Exists)
                                                //{
                                                //    info.Delete(true);
                                                //}
                                            }

                                            if (newsForMatch.Gallery.GalleryPhotos != null)
                                            {
                                                //newsForMatch.Gallery.GalleryPhotos.Clear();
                                            }
                                            if (newsForMatch.Id == default)
                                            {
                                                _newRepository.Add(newsForMatch);

                                            }
                                            else
                                            {
                                                _newRepository.Update(newsForMatch);
                                            }

                                            foreach (var photo in formGalleryFiles)
                                            {
                                                string photoName = await _photoService.PlacePhoto(photo, "gallery", gallery.Id);
                                                if (photoName != "0" && photoName != "1" && photoName != "2")
                                                {

                                                    gallery.GalleryPhotos.Add(new GalleryPhotos()
                                                    {
                                                        Gallery = gallery,
                                                        Photo = new Photo()
                                                        {
                                                            Name = photoName,
                                                            Path = "galleries/" + gallery.Id + "/" + photoName,
                                                            DisplayOrder = 1,
                                                            CreatedOnUtc = DateTime.UtcNow
                                                        }
                                                    });
                                                }
                                                else
                                                {
                                                    MessageCode = 1;
                                                    Message = "Galeride bazı fotoğraflar büyüktü. Ben de kaydetmedim. Sınır 2MB.";
                                                }

                                            }

                                        }


                                    }

                                    if (newsForMatch.Id == default)
                                    {
                                        _newRepository.Add(newsForMatch);

                                    }
                                    else
                                    {
                                        _newRepository.Update(newsForMatch);
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
                    catch (Exception)
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