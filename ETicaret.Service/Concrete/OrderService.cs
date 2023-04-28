using ETicaret.Data;
using ETicaret.Data.Abstract;
using ETicaret.Data.Concrete;
using ETicaret.Entities;
using ETicaret.Service.Abstract;

namespace ETicaret.Service.Concrete
{
    public class OrderService : OrderRepository, IOrderService
    {
        private IOrderRepository _orderRepository;

        public OrderService(DatabaseContext _context, IOrderRepository orderRepository) : base(_context)
        {
            _orderRepository = orderRepository;
        }


        /*private readonly IUnitOfWork _unitofwork;
        public OrderService(DatabaseContext _context, IUnitOfWork unitofwork) : base(_context)
        {
            _unitofwork = unitofwork;
        }*/
        public void Create(Order entity)
        {
            _orderRepository.Add(entity);
            _orderRepository.SaveChanges();
            /*_unitofwork.Orders.Add(entity);
            _unitofwork.Save();*/
        }

        public List<Order> GetOrders(string userId)
        {
            return _orderRepository.GetOrders(userId);
            //return _unitofwork.Orders.GetOrders(userId);
        }
    }
}
