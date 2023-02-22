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
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class BrandsController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // GET: BrandsController
        public ActionResult Index()
        {
            var marka = new BrandListModel()
            {
                Brands = _brandService.GetAll()
            };
            return View(marka);
        }

        // GET: BrandsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BrandsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrandsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(BrandModel brand, IFormFile? Logo, string filePath = "/Img/Brands/")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    brand.Logo = await FileHelper.FileLoaderAsync(Logo, filePath);
                    var marka = new Brand()
                    {
                        Name = brand.Name,
                        Description = brand.Description,
                        Logo = brand.Logo,
                        IsActive = brand.IsActive,
                        CreateDate = DateTime.Now,
                    };
                    await _brandService.AddAsync(marka);
                    await _brandService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(brand);
        }


        // POST: BrandsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2(Brand brand, IFormFile? Logo)
        {
            if (ModelState.IsValid) // Model class ımız olan brand nesnesinin validasyon için koyduğumuz kurallarınıa (örneğin marka adı required-boş geçilemez gibi) uyulmuşsa
            {
                try
                {
                    brand.Logo = await FileHelper.FileLoaderAsync(Logo);
                    _brandService.Add(brand);
                    _brandService.SaveChanges();
                    return RedirectToAction("Create", "Products");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }

            return View(brand);
        }

        // GET: BrandsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var model = await _brandService.FindAsync(id);
            var marka = new BrandModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description= model.Description,
                Logo = model.Logo,
                IsActive = model.IsActive,
                CreateDate = DateTime.Now
            };
            return View(marka);
        }

        // POST: BrandsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, BrandModel brands, IFormFile? Logo, string filePath = "/Img/Brands/")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Logo is not null) brands.Logo = await FileHelper.FileLoaderAsync(Logo, filePath);
                    var model = new Brand()
                    {
                        Id = brands.Id,
                        Name = brands.Name,
                        Description = brands.Description,
                        Logo = brands.Logo,
                        IsActive = brands.IsActive,
                        CreateDate = DateTime.Now
                    };
                    _brandService.Update(model);
                    await _brandService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(brands);
        }

        // GET: BrandsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _brandService.FindAsync(id);
            var marka = new BrandModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Logo = model.Logo,
                IsActive = model.IsActive,
                CreateDate = DateTime.Now
            };
            return View(marka);
        }

        // POST: BrandsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Brand brands, string filePath = "/Img/Brands/")
        {
            try
            {
                FileHelper.FileRemover(brands.Logo, filePath);
                _brandService.Delete(brands);
                await _brandService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
