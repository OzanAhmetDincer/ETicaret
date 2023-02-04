using ETicaret.Data.Abstract;
using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Data.Concrete
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DatabaseContext _context) : base(_context)
        {
        }

        public async Task<Category> GetCategoryByProductsAsync(int id)
        {
            return await context.Categories.Where(k => k.Id == id).AsNoTracking().Include(p => p.Products).FirstOrDefaultAsync();// context üzerindeki kategorilerden id si bu metoda gönderilen id ile eşleşen kaydı bul ve bu kaydı izleme (AsNoTracking), bulduğun kategorinin ürünlerini de include ile join le birleştir ve ilk kaydı metodun çağrıldığı yere döndür.
        }
    }
}
