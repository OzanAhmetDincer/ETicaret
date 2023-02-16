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
            var orders = context.Orders.Include(i => i.OrderItems).ThenInclude(i => i.Product).AsQueryable();
            if (!string.IsNullOrEmpty(userId))
            {
                orders = orders.Where(i=>i.UserId==userId);
            }
            return orders.ToList();
        }
    }
}
