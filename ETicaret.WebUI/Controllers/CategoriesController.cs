using ETicaret.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var model = await _categoryService.GetCategoryByProductsAsync(id);
            return View(model);
        }
    }
}
