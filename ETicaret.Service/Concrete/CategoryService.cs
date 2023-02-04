using ETicaret.Data;
using ETicaret.Data.Concrete;
using ETicaret.Service.Abstract;

namespace ETicaret.Service.Concrete
{
    public class CategoryService : CategoryRepository, ICategoryService
    {
        public CategoryService(DatabaseContext _context) : base(_context)
        {
        }
    }
}
