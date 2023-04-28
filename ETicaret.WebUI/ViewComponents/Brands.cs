using ETicaret.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.ViewComponents
{
    public class Brands : ViewComponent
    {
        private readonly IBrandService _brandService;

        public Brands(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _brandService.GetAllAsync(c => c.IsActive);
            return View(model);
        }
    }
}
