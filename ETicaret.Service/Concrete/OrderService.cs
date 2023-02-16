using ETicaret.Data;
using ETicaret.Data.Abstract;
using ETicaret.Data.Concrete;
using ETicaret.Entities;
using ETicaret.Service.Abstract;

namespace ETicaret.Service.Concrete
{
    public class OrderService : OrderRepository, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(DatabaseContext _context, IOrderRepository orderRepository) : base(_context)
        {
            _orderRepository = orderRepository;
        }
        public void Create(Order entity)
        {
            _orderRepository.Add(entity);
            _orderRepository.SaveChanges();
        }

        public List<Order> GetOrders(string userId)
        {
            return _orderRepository.GetOrders(userId);
        }
    }
}
