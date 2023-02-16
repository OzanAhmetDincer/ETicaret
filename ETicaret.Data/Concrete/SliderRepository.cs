using ETicaret.Data.Abstract;
using ETicaret.Entities;

namespace ETicaret.Data.Concrete
{
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {
        public SliderRepository(DatabaseContext _context) : base(_context)
        {
        }
    }
}
