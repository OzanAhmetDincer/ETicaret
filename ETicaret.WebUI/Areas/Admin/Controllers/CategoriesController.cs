using ETicaret.Entities;
using ETicaret.Service.Abstract;
using ETicaret.WebUI.Areas.Admin.Models;
using ETicaret.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: CategoriesController
        public ActionResult Index()
        {
            var model = new CategoryListModel()
            {
                Categories = _categoryService.GetAll()
            };
            return View(model);
        }

        // GET: CategoriesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoriesController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.ParentId = new SelectList(await _categoryService.GetAllAsync(), "Id" , "Name");
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(CategoryModel category, IFormFile? Image, string filePath = "/Img/Categories/")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null) category.Image = await FileHelper.FileLoaderAsync(Image, filePath);
                    var kategori = new Category()
                    {
                        Name = category.Name,
                        Description = category.Description,
                        Image = category.Image,
                        CreateDate = DateTime.Now,
                        IsActive = category.IsActive,
                        IsTopMenu = category.IsTopMenu,
                        ParentId = category.ParentId,
                        OrderNo = category.OrderNo
                    };
                    await _categoryService.AddAsync(kategori);
                    await _categoryService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ViewBag.ParentId = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create2(Category category, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null) category.Image = await FileHelper.FileLoaderAsync(Image);
                    await _categoryService.AddAsync(category);
                    await _categoryService.SaveChangesAsync();
                    return RedirectToAction("Create", "Products");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ViewBag.ParentId = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");
            return View(category);
        }

        // GET: CategoriesController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var model = await _categoryService.FindAsync(id);
            var kategori = new CategoryModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Image = model.Image,
                CreateDate = DateTime.Now,
                IsActive = model.IsActive,
                IsTopMenu = model.IsTopMenu,
                ParentId = model.ParentId,
                OrderNo = model.OrderNo
            };
            ViewBag.ParentId = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");
            return View(kategori);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, CategoryModel category, IFormFile? Image, string filePath = "/Img/Categories/")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null)
                    {
                        category.Image = await FileHelper.FileLoaderAsync(Image, filePath);
                    }
                    var model = new Category()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description,
                        Image = category.Image,
                        CreateDate = DateTime.Now,
                        IsActive = category.IsActive,
                        IsTopMenu = category.IsTopMenu,
                        ParentId = category.ParentId,
                        OrderNo = category.OrderNo
                    };
                    _categoryService.Update(model);
                    await _categoryService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ViewBag.ParentId = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");
            return View(category);
        }

        // GET: CategoriesController/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _categoryService.Find(id);
            var kategori = new CategoryModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Image = model.Image,
                CreateDate = DateTime.Now,
                IsActive = model.IsActive,
                IsTopMenu = model.IsTopMenu,
                ParentId = model.ParentId,
                OrderNo = model.OrderNo
            };
            return View(kategori);
        }

        // POST: CategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Category category, string filePath = "/Img/Categories/")
        {
            try
            {
                FileHelper.FileRemover(category.Image, filePath);
                _categoryService.Delete(category);
                await _categoryService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
