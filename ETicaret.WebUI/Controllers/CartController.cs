using ETicaret.Service.Abstract;
using ETicaret.WebUI.Identity;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;
        private IOrderService _orderService;
        private UserManager<User> _userManager;
        public CartController(IOrderService orderService, ICartService cartService, UserManager<User> userManager)
        {
            _cartService = cartService;
            _orderService = orderService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            // Bu metotda login kullanıcının cart bilgilerini ekrana getiririz. Bunu ilk başta kullanıcının userId'sini alırız, userId üzerinden de cartId'sini alırız. cartId bilgisi ile de cartItem'daki ilgili cart ile ilişkilendirilen ürünleri ve bilgilerini alırız. 
            //_userManager.GetUserId(User) ile userManager üzerinden GetUserId metodu ile ilgili kullanıcının userId'si gelmiş olur. Burada GetUserId metodu içerisine verdiğimiz "User" veri tabanından aldığımız bir user değil sadece kullanıcı için oluşturulan bir sessiondur. Bu ıd üzerinden de _cartService içerisindeki cartId'sine ulaşırız.
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
            return View(new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()// Her gelen cartItem üzerinden new ile yeni bir tane obje oluşturucaz
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.Image,
                    Quantity = i.Quantity
                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.AddToCart(userId, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.DeleteFromCart(userId, productId);// Hangi userId'ye ait olan cart içerisinden hangi ürünün(productId) silineceğini tanımladık
            return RedirectToAction("Index");
        }
    }
}
