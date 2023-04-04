using ETicaret.Data.Abstract;
using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Data.Concrete
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DatabaseContext _context) : base(_context)
        {
        }
        public List<Order> GetOrders(string userId)
        {
            // Veri tabanında ki Orders'a alırız buradan Include ile OrderItems'a geçiş yaparız. OrderItems tablosundan da ThenInclude ile ilgili Product kayıtlarını alıcaz
            var orders = context.Orders.Include(i => i.OrderItems).ThenInclude(i => i.Product).AsQueryable();
            if (!string.IsNullOrEmpty(userId))
            {
                // Eğer gönderilen userId boş değilse filtreleme işlemi yaparız. Yani bize bir userId geliyorsa yukarıda ki sorgunun üzerine aşağıdaki where bloğunuda ekleyecez ve kullanıcının Order'larını getirecez. userId null yada boş geiyorsa sadece yukarıdaki metotlar çalışır ve tüm order'lar gelir
                orders = orders.Where(i=>i.UserId==userId);
            }
            return orders.ToList();
        }
    }
}
