using ETicaret.Entities;

namespace ETicaret.WebUI.Models
{
    public class SearchModel
    {
        public string search { get; set; }
        public List<Product>? Products { get; set; }
    }
}
