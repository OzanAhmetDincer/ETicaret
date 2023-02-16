using ETicaret.Data.Abstract;
using ETicaret.Entities;

namespace ETicaret.Data.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext _context) : base(_context)
        {
        }
    }
}
