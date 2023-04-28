using ETicaret.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.ViewComponents
{
    public class Categories1 : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public Categories1(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _categoryService.GetAllAsync(c => c.IsActive);
            return View(model);
        }
    }
}
