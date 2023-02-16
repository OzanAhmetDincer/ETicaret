using ETicaret.Data.Abstract;
using ETicaret.Entities;

namespace ETicaret.Service.Abstract
{
    public interface IOrderService : IOrderRepository
    {
        void Create(Order entity);
        List<Order> GetOrders(string userId);
    }
}
