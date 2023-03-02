using ETicaret.Entities;

namespace ETicaret.WebUI.Models
{
    public class BrandPageViewModel
    {
        public Brand Brand { get; set; }
        public List<Product>? Products { get; set; }
    }
}
