using ETicaret.Data.Abstract;
using ETicaret.Entities;

namespace ETicaret.Data.Concrete
{
    public class BrandRepository : Repository<Brand>,IBrandRepository
    {
        public BrandRepository(DatabaseContext _context) : base(_context)
        {
        }
    }
}
