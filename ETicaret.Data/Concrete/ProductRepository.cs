using ETicaret.Data.Abstract;
using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Data.Concrete
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DatabaseContext _context) : base(_context)
        {
        }

        public async Task<IEnumerable<Product>> GetAllProductsByCategoriesBrandsAsync()
        {
            return await context.Products.Include(c => c.Category).Include(b => b.Brand).AsNoTracking().ToListAsync();// bu metot geriye ürün listesi dönecek ve listedeki her bir ürüne o ürünün kategorisi ve markası da dahil edilecek. context üzeriden Products a erişim ef core un include metoduyla hem ürünün kategorisini hem de markasını products a dahil edip en son tolistasync diyerek listeleyip verileri döndürüyoruz.
        }
        public async Task<Product> GetProductByCategoriesBrandsAsync(int id)
        {
            return await context.Products.Include(c => c.Category).Include(b => b.Brand).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
