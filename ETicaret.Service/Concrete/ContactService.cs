using ETicaret.Data;
using ETicaret.Data.Concrete;
using ETicaret.Service.Abstract;

namespace ETicaret.Service.Concrete
{
    public class ContactService : ContactRepository, IContactService
    {
        public ContactService(DatabaseContext _context) : base(_context)
        {
        }
    }
}
