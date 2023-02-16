using ETicaret.Data;
using ETicaret.Data.Concrete;
using ETicaret.Service.Abstract;

namespace ETicaret.Service.Concrete
{
    public class BrandService : BrandRepository , IBrandService
    {
        public BrandService(DatabaseContext _context) : base(_context)
        {
        }
    }
}
