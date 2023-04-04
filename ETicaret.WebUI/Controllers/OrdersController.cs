using ETicaret.Service.Abstract;
using ETicaret.WebUI.Identity;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Controllers
{
    public class OrdersController : Controller
    {
        private IOrderService _orderService;
        private UserManager<User> _userManager;
        public OrdersController(IOrderService orderService, UserManager<User> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }
        public IActionResult GetOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetOrders(userId);

            // Aşağıda OrderListModel türünde orderListModel isminde bir liste oluşturduk. Bu kullanıcının vermiş olduğu tüm siparişleri barındıracak
            var orderListModel = new List<OrderListModel>();
            // OrderListModel türünde orderModel isminde bir tane nesne oluşturduk. Sonrasında bu nesneyi foreach döngüsü içerisinde her sipariş için yeniden oluşturup içerisine yukarıda aldığımız kullanıcının sipariş bilgilerini doldurup ekrana göndericez.
            OrderListModel orderModel;
            foreach (var order in orders)
            {
                orderModel = new OrderListModel();

                orderModel.OrderId = order.Id;
                orderModel.OrderNumber = order.OrderNumber;
                orderModel.OrderDate = order.OrderDate;
                orderModel.Phone = order.Phone;
                orderModel.FirstName = order.FirstName;
                orderModel.LastName = order.LastName;
                orderModel.Email = order.Email;
                orderModel.Address = order.Address;
                orderModel.City = order.City;
                orderModel.OrderState = order.OrderState;
                orderModel.PaymentType = order.PaymentType;

                // Verilen siparişlerin ürün bilgilerinide OrderItems'ı Select sorgusu ile tek tek dolaşıcaz, dolaşırkende her bir sipariş bilgilerini OrderItemModel içerisinde doldurup ToList metodu ile listeye çevirecez.
                orderModel.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                {
                    OrderItemId = i.Id,
                    Name = i.Product.Name,
                    Price = (double)i.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.Product.Image
                }).ToList();

                // Bilgileri oluşturulan her bir Order'ı da yukarıda liste şeklinde tanımladığımız orderListModel içerisine Add metodu ile ekleriz. Ekleme işlemi olduktan sonra foreach tekrar başa dönüm yeni bir orderModel oluşturur ve bilgiler doldurulur. Veri tabanında kaç tane sipariş varsa onları orderListModel'e ekler.
                orderListModel.Add(orderModel);
            }
            // Oluşturulan orderListModel'i GetOrders.cshtml sayfasına göndeririz. 
            return View("GetOrders", orderListModel);
        }
    }
}
