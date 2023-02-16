using ETicaret.Data.Abstract;
using ETicaret.Entities;

namespace ETicaret.Data.Concrete
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(DatabaseContext _context) : base(_context)
        {
        }
    }
}
