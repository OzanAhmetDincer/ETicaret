using ETicaret.Service.Abstract;
using ETicaret.Service.Concrete;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Controllers
{
    public class BrandsController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IProductService _productService;
        public BrandsController(IBrandService brandService, IProductService productService)
        {
            _brandService = brandService;
            _productService = productService;
        }
        public async Task<IActionResult> IndexAsync(int id)
        {
            var model = new BrandPageViewModel()
            {
                Brand = await _brandService.FindAsync(id),
                Products = await _productService.GetAllAsync(p => p.IsActive && p.BrandId == id)
            };
            return View(model);
        }
    }
}
