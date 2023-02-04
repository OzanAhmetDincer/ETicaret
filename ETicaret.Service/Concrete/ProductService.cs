using ETicaret.Data;
using ETicaret.Data.Concrete;
using ETicaret.Service.Abstract;

namespace ETicaret.Service.Concrete
{
    public class ProductService : ProductRepository, IProductService
    {
        public ProductService(DatabaseContext _context) : base(_context)
        {
        }
    }
}
