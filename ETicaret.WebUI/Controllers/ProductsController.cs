using ETicaret.Service.Abstract;
using ETicaret.Service.Concrete;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Search(string q)
        {
            var urun = await _productService.GetAllAsync(p => p.IsActive && p.Name.Contains(q));
            var model = new SearchModel()
            {
                search = q,
                Products = urun
            };
            return View(model);
            //var model = await _productService.GetAllAsync(p => p.IsActive && p.Name.Contains(q));
            //return View(model);
        }

        public async Task<ActionResult> DetailAsync(int id)
        {
            var product = await _productService.GetProductByCategoriesBrandsAsync(id);
            var model = new ProductDetailViewModel()
            {
                Product = product,
                Products = await _productService.GetAllAsync(p => p.IsActive && p.Id != id)
            };

            return View(model);
        }
    }
}
