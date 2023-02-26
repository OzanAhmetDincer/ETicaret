﻿using ETicaret.Entities;
using ETicaret.Service.Abstract;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ETicaret.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISliderService _sliderService;
        private readonly IBrandService _brandService;
        private readonly IContactService _contactService;
        private readonly IService<News> _newsService;

        public HomeController(IProductService productService, ICategoryService categoryService, ISliderService sliderService, IBrandService brandService, IContactService contactService, IService<News> newsService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _sliderService = sliderService;
            _brandService = brandService;
            _contactService = contactService;
            _newsService = newsService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var model = new HomePageViewModel()
            {
                Sliders = await _sliderService.GetAllAsync(),
                Products = await _productService.GetAllAsync(),
                Brands = await _brandService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                News = await _newsService.GetAllAsync()
            };
            return View(model);
        }
        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}