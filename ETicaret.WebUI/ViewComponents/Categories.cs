using ETicaret.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.ViewComponents
{
    public class Categories : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public Categories(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // var model = await _categoryService.GetAllAsync();
            var model = await _categoryService.GetAllAsync(c => c.IsActive && c.IsTopMenu);
            return View(model);
        }
    }
}
