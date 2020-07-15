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
    public class NewsController : BaseController<NewsController>
    {
        private readonly INewRepository _newRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IPhotoService _photoService;
        public NewsController(INewRepository newRepository, ISeasonRepository seasonRepository, IPhotoService photoService, ISettingRepository settingRepository) : base(settingRepository)
        {
            _newRepository = newRepository;
            _seasonRepository = seasonRepository;
            _photoService = photoService;
        }
        public IActionResult List(FilterNewGetModel filterModel)
        {
            var news = _newRepository.GetGeneralNews();
            if (filterModel.DateFilterStart != default)
                news = news.Where(x => x.CreatedOnUtc.Date > filterModel.DateFilterStart.ToUniversalTime());
            if (filterModel.DateFilterEnd != default)
                news = news.Where(x => x.CreatedOnUtc.Date < filterModel.DateFilterEnd.ToUniversalTime());
            var model = new ListNewViewModel()
            {
                News = news,
                Filters = filterModel
            };
            return View(model);
        }

        public IActionResult Create()
        {
            var seasons = _seasonRepository.GetActiveSeasons();
            var model = new CreateNewViewModel()
            {
                New = new New(),
                Seasons = seasons
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNewPostModel model)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    var newsForMatch = new New();
                    newsForMatch.Season = _seasonRepository.Get(model.SeasonId);
                    var videoPicture = model.VideoPicture;
                    var newsPicture = model.NewsPicture;
                    bool videoPictureDefined = (videoPicture != null && videoPicture.Length > 0);
                    bool newsPictureDefined = (newsPicture != null && newsPicture.Length > 0);
                    if (videoPictureDefined || newsPictureDefined)
                    {
                        if (videoPictureDefined) //Deprecated
                        {
                        }

                        if (newsPictureDefined)
                        {
                            if (newsForMatch.Id != default && newsForMatch.CoverPhotoId != default)
                            {
                                var oldPhoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\news_pictures", newsForMatch.CoverPhoto.Name);
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
                                return RedirectToAction("Create", "News");
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

                    return RedirectToAction("List", "News");


                }
                catch (Exception)
                {
                    MessageCode = 0;
                    Message = "Bir şeylerde sıçtık, yaptığın her şey boşa gitti :(";
                    scope.Dispose();
                    return RedirectToAction("List", "News");

                }
            }
        }


        public IActionResult Edit(int newsId)
        {
            var seasons = _seasonRepository.GetActiveSeasons();
            var newToBeEdit = _newRepository.Get(newsId);
            if (newToBeEdit != null)
            {

                var model = new EditNewViewModel()
                {
                    New = newToBeEdit,
                    Seasons = seasons
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("List", "News");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditNewPostModel model)
        {

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    var newsForMatch = _newRepository.Get(model.NewId);
                    if (newsForMatch != null)
                    {

                        newsForMatch.Season = _seasonRepository.Get(model.SeasonId);
                        var videoPicture = model.VideoPicture;
                        var newsPicture = model.NewsPicture;
                        bool videoPictureDefined = (videoPicture != null && videoPicture.Length > 0);
                        bool newsPictureDefined = (newsPicture != null && newsPicture.Length > 0);
                        if (videoPictureDefined || newsPictureDefined)
                        {
                            if (videoPictureDefined) //Deprecated
                            {
                            }

                            if (newsPictureDefined)
                            {
                                if (newsForMatch.Id != default && newsForMatch.CoverPhotoId != default)
                                {
                                    var oldPhoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\news_pictures", newsForMatch.CoverPhoto.Name);
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
                                    return RedirectToAction("Create", "News");
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
                            newsForMatch.CreatedOnUtc = DateTime.UtcNow;


                            _newRepository.Update(newsForMatch);



                        }

                        scope.Complete();
                        MessageCode = 1;
                        Message = "Her şeyi hallettik gibi, iyi işti !";

                        return RedirectToAction("List", "News");

                    }
                    else
                    {
                        scope.Dispose();
                        MessageCode = 0;
                        Message = "Ne yapmaya çalıştığını tam anlayamadım, ama tekrar denemesen iyi olur.";

                        return RedirectToAction("List", "News");
                    }


                }
                catch (Exception)
                {
                    MessageCode = 0;
                    Message = "Bir şeylerde sıçtık, yaptığın her şey boşa gitti :(";
                    scope.Dispose();
                    return RedirectToAction("List", "News");

                }
            }
        }
    }
}