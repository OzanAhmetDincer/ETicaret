﻿using ETicaret.Entities;
using ETicaret.Service.Abstract;
using ETicaret.WebUI.Areas.Admin.Models;
using ETicaret.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class NewsController : Controller
    {
        private readonly IService<News> _service;

        public NewsController(IService<News> service)
        {
            _service = service;
        }

        // GET: NewsController
        public ActionResult Index()
        {
            var haber = new NewsListModel()
            {
                Haberler = _service.GetAll()
            };

            return View(haber);
        }

        // GET: NewsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NewsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(NewsModel news, IFormFile? Image, string filePath = "/Img/News/")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null) news.Image = await FileHelper.FileLoaderAsync(Image, filePath);
                    var model = new News()
                    {
                        Name = news.Name,
                        Content = news.Content,
                        Image = news.Image,
                        CreateDate = DateTime.Now
                    };
                    await _service.AddAsync(model);
                    await _service.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(news);
        }

        // GET: NewsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var model = await _service.FindAsync(id);
            var haber = new NewsModel()
            {
                Id = model.Id,
                Name = model.Name,
                Content = model.Content,
                Image = model.Image,
                CreateDate = DateTime.Now
            };
            return View(haber);
        }

        // POST: NewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, NewsModel news, IFormFile? Image, string filePath = "/Img/News/")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null) news.Image = await FileHelper.FileLoaderAsync(Image,filePath);
                    var model = new News()
                    {
                        Id = news.Id,
                        Name = news.Name,
                        Content = news.Content,
                        Image = news.Image,
                        CreateDate = DateTime.Now
                    };
                     _service.Update(model);
                    await _service.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(news);
        }

        // GET: NewsController/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _service.Find(id);
            var haber = new NewsModel()
            {
                Id = model.Id,
                Name = model.Name,
                Content = model.Content,
                Image = model.Image,
                CreateDate = DateTime.Now
            };
            return View(haber);
        }

        // POST: NewsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, News news, string filePath = "/Img/News/")
        {
            try
            {
                FileHelper.FileRemover(news.Image, filePath);
                _service.Delete(news);
                await _service.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
