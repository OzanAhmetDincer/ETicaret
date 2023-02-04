using ETicaret.Data;
using ETicaret.Data.Concrete;
using ETicaret.Entities;
using ETicaret.Service.Abstract;

namespace ETicaret.Service.Concrete
{
    public class Service<T> : Repository<T>, IService<T> where T : class, IEntity, new()
    {
        public Service(DatabaseContext _context) : base(_context)
        {
        }
    }
}
