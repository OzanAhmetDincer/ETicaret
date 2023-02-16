using ETicaret.Data;
using ETicaret.Data.Concrete;
using ETicaret.Service.Abstract;

namespace ETicaret.Service.Concrete
{
    public class SliderService : SliderRepository, ISliderService
    {
        public SliderService(DatabaseContext _context) : base(_context)
        {
        }
    }
}
