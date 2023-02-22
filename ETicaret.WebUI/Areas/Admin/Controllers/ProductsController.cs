using ETicaret.Entities;
using ETicaret.Service.Abstract;
using ETicaret.Service.Concrete;
using ETicaret.WebUI.Areas.Admin.Models;
using ETicaret.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        
        public ProductsController(IProductService productService, IBrandService brandService, ICategoryService categoryService)
        {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
        }
        // GET: ProductsController
        public ActionResult Index()
        {
            var urun = new ProductListModel()
            {
                Products = _productService.GetAll()
            };
            return View(urun);
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.CategoryId = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandService.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(ProductModel product, IFormFile? Image, string filePath = "/Img/Products/")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null) product.Image = await FileHelper.FileLoaderAsync(Image, filePath);
                    var urun = new Product()
                    {
                        Name = product.Name,
                        Description= product.Description,
                        Image= product.Image,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        IsActive = product.IsActive,
                        IsHome= product.IsHome,
                        Price = product.Price,
                        Stock = product.Stock,
                        CategoryId = product.CategoryId,
                        BrandId = product.BrandId
                    };
                    await _productService.AddAsync(urun);
                    await _productService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(product);
        }

        // GET: ProductsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var model = await _productService.FindAsync(id);
            var urun = new ProductModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Image = model.Image,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsActive = model.IsActive,
                IsHome = model.IsHome,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId
            };
            ViewBag.CategoryId = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandService.GetAllAsync(), "Id", "Name");
            return View(urun);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, ProductModel product, IFormFile? Image, bool? resmisil, string filePath = "/Img/Products/" )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (resmisil == true)
                    {
                        FileHelper.FileRemover(product.Image, filePath: "/wwwroot/Img/Products/");
                        product.Image = string.Empty;
                    }
                    if (Image is not null)
                    {
                        FileHelper.FileRemover(product.Image, filePath: "/wwwroot/Img/Products/");
                        product.Image = await FileHelper.FileLoaderAsync(Image, filePath: "/wwwroot/Img/Products/");
                    }
                    var urun = new Product()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Image = product.Image,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        IsActive = product.IsActive,
                        IsHome = product.IsHome,
                        Price = product.Price,
                        Stock = product.Stock,
                        CategoryId = product.CategoryId,
                        BrandId = product.BrandId
                    };
                    _productService.Update(urun);
                    await _productService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(product);
        }

        // GET: ProductsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _productService.FindAsync(id);
            var urun = new ProductModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Image = model.Image,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsActive = model.IsActive,
                IsHome = model.IsHome,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId
            };
            ViewBag.CategoryId = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandService.GetAllAsync(), "Id", "Name");
            return View(urun);
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product product)
        {
            try
            {
                FileHelper.FileRemover(product.Image, filePath: "/wwwroot/Img/Products/");
                _productService.Delete(product);
                _productService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
