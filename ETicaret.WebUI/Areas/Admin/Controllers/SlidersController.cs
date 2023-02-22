using ETicaret.Entities;
using ETicaret.Service.Abstract;
using ETicaret.Service.Concrete;
using ETicaret.WebUI.Areas.Admin.Models;
using ETicaret.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class SlidersController : Controller
    {
        private readonly ISliderService _sliderService;

        public SlidersController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        // GET: SlidersController
        public ActionResult Index()
        {
            var slider = new SliderListModel()
            {
                Sliders = _sliderService.GetAll()
            };

            return View(slider);
        }

        // GET: SlidersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SlidersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SlidersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(SliderModel sliders, IFormFile? Image, string filePath = "/Img/Sliders/")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null) sliders.Image = await FileHelper.FileLoaderAsync(Image, filePath);
                    var model = new Slider()
                    {
                        Title = sliders.Title,
                        Description = sliders.Description,
                        Link = sliders.Link,
                        Image = sliders.Image
                    };
                    await _sliderService.AddAsync(model);
                    await _sliderService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(sliders);
        }

        // GET: SlidersController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var model = await _sliderService.FindAsync(id);
            var slide = new SliderModel()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Link = model.Link,
                Image = model.Image
            };
            return View(slide);
        }

        // POST: SlidersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(SliderModel sliders, IFormFile? Image, string filePath = "/Img/Sliders/")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null) sliders.Image = await FileHelper.FileLoaderAsync(Image, filePath);
                    var model = await _sliderService.FindAsync(sliders.Id);
                    model.Title = sliders.Title;
                    model.Description = sliders.Description;
                    model.Link = sliders.Link;
                    model.Image = sliders.Image;
                    _sliderService.Update(model);
                    await _sliderService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(sliders);
        }

        // GET: SlidersController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _sliderService.FindAsync(id);
            var slide = new SliderModel()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Link = model.Link,
                Image = model.Image
            };
            return View(slide);
        }

        // POST: SlidersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Slider sliders, string filePath = "/Img/Sliders/")
        {
            try
            {
                FileHelper.FileRemover(sliders.Image, filePath);
                _sliderService.Delete(sliders);
                await _sliderService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
