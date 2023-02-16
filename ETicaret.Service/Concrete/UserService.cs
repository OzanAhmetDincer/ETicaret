using ETicaret.Data;
using ETicaret.Data.Concrete;
using ETicaret.Service.Abstract;

namespace ETicaret.Service.Concrete
{
    public class UserService : UserRepository, IUserService
    {
        public UserService(DatabaseContext _context) : base(_context)
        {
        }
    }
}
