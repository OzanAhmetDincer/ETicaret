using ETicaret.Data.Abstract;
using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Data.Concrete
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(DatabaseContext _context) : base(_context)
        {
        }

        public void ClearCart(int cartId)
        {
            // CartItems üzerinden gönderilen CartId bilgisine göre silme işlemini yaparız
            var cmd = @"delete from CartItems where CartId=@p0"; 
            context.Database.ExecuteSqlRaw(cmd,cartId);
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            var cmd = @"delete from CartItems where CartId=@p0 and ProductId=@p1";// CartItems dan silme işlemi yapılacak. Kriter olarak da veri tabanındaki CartId'ye p0 yani ilk obje yani ExecuteSqlRaw içerisindeki dışarıdan göndereceğimiz cartId bilgisi ve veri tabanındaki ProductId alanı ise ikinci obje(p1) olan productId bilgisi
            context.Database.ExecuteSqlRaw(cmd, cartId, productId);
        }

        public Cart GetByUserId(string userId)
        {
            // İlgili Carts objesi üzerinden CartItems'ları yüklüyoruz buradanda başka bir entity'e geçtiğimiz için ThenInclude ile CartItem içerisindeki Product tablolarına ulaşırız bu bize tüm kayıtları getirir, sonrasında FirstOrDefault ile ilk kayıta ulaşırız. Burada da içerisine yazdığımız sorgu ile kayıtlı olan UserId ile dışarıdan gönderilen userId'si aynı olan ilk kaydı getir dedik.
            return context.Carts.Include(i => i.CartItems).ThenInclude(i => i.Product).FirstOrDefault(i => i.UserId == userId);
        }
        public override void Update(Cart entity)
        {
            context.Carts.Update(entity);
        }
    }
}
