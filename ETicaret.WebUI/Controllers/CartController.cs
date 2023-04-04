using ETicaret.Service.Abstract;
using ETicaret.WebUI.Identity;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Iyzipay.Request;
using Iyzipay.Model;
using Iyzipay;
using ETicaret.Entities;


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

        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));// Kullanıcının cart bilgisini getirdik

            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.Image,
                    Quantity = i.Quantity

                }).ToList()
            };

            return View(orderModel);
        }

        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            // Ödeme işleminde kredi kartı entegresyonu veren belirli ücretsi api'ler var. https://stripe.com/ veya https://www.iyzico.com/ gibi sayfalardan bu hizmeti alabiliriz. Biz bu projede iyzipay kullandık. Bu sitede "geliştirici" ve "sandbox" bölümleri var. Sandbox bölümü bize test ortamı sunacak olan bir sayfa. Bu sayfayı kullanmak için üyelik oluşturmak gerekiyor. 10 dklık bir mail alıp, üyelik sayfasında bilgileri rastgele girebilirsin. Telefon numaranıda farklı yaz. Sonrasında telefona onay kodu gelecek orayada 123456 yazarak giriş yap. Sonuçta test ortamında çalışacaz. Kodu yazdıktan sonra üyeliğin oluşur. Açılan sayfada ayarlar kısmında firma ayarları var. Buradaki API anahtarı ve Güvenlik anahtarı bilgileri ile entegrasyon yapıcaz. Geliştirici sayfasında tüm dökümanlar var. Biz bu sayfadan iyzico'nun github sayfasına geçiş yapıp kendi projemizi yaptığımız programlama diline ait dökümana gideriz(iyzipay-dotnet yazan döküman). Bu sayfada bize detay bilgileri veriyor, proje veriyor ve ne yapmamız gerekktiğini anlatıyor. Bizim kendi projemize ilk aşama Iyzipay nuget yüklememiz gerekiyor. Sonrasında "Newtonsoft.json" kütüphanesinin olması gerekiyor ki bu genelde yüklü. Diğer bilgilerde de temel kullanımı anlatıyor. O sayfada verilen örnek bilgileri kendi uygulamandan gelen bilgiler ile değiştirmen gerekiyor.         
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var cart = _cartService.GetCartByUserId(userId);// Kullanıcının cart bilgilerinin tekrar almamızın sebebi order(sipariş) ekranında ürünlerde tekrardan bir değişiklik yaparsa o bilgileride veri tabanına aktarmak. Biz bunları sipariş kaydına çevireceğimiz zaman hangi ürünleri aldığınıda SaveOrder içerisinde kayıt etmemiz gerekir. Bu bilgileri sayfa üzerinden <input type="hidden"> şeklinde de post metoduna alabiliriz. Aşağıdaki CartModel olarakta alabiliriz.

                model.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.Name,
                        Price = (double)i.Product.Price,
                        ImageUrl = i.Product.Image,
                        Quantity = i.Quantity
                    }).ToList()
                };
                // Sayfadan gelen model içerisinde cart bilgileri var. Aşağıda tanımladığımız PaymentProcess metoduna model bilgisini veririz ve bilgileri işleriz
                var payment = PaymentProcess(model);

                if (payment.Status == "success")
                {
                    // Ödemeden oluşturulan değer "success" ise bizi return View ile Success.cshtml sayfasına yönlendirecek
                    // Kullanıcının göndermiş olduğu bilgileri bir order'a çevirmemiz gerekiyor. Aşağıda tanımladığımız "SaveOrder" metodu ile order bilgileri ve cart bilgilerini veri tabanına kaydederiz. payment içerisinden "ConversationId" ve "PaymentId" bilgilerini alırız. Bunlarıda veri tabanına kaydederiz ki daha sonradan api ile iletişime geçileceği zaman bu bilgiler lazım olacak
                    SaveOrder(model, payment, userId);
                    // Sipariş verildikten sonra ilgili kullanıcının cart'ını(sepet) ClearCart metodu ile temizlememiz gerekiyor.
                    ClearCart(model.CartModel.CartId);
                    return View("Success");
                }
                else
                {
                    var msg = new AlertMessage()
                    {
                        Message = $"{payment.ErrorMessage}",
                        AlertType = "danger"
                    };

                    TempData["message"] = JsonConvert.SerializeObject(msg);
                }
            }
            return View(model);
        }

        private void ClearCart(int cartId)
        {
            _cartService.ClearCart(cartId);
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order();

            order.OrderNumber = new Random().Next(111111, 999999).ToString();
            order.OrderState = Order.EnumOrderState.completed;
            order.PaymentType = Order.EnumPaymentType.CreditCard;
            order.PaymentId = payment.PaymentId;
            order.ConversationId = payment.ConversationId;
            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.UserId = userId;
            order.Address = model.Address;
            order.Phone = model.Phone;
            order.Email = model.Email;
            order.City = model.City;
            order.Note = model.Note;

            order.OrderItems = new List<Entities.OrderItem>();// OrderItem türünde bir liste oluştururuz. Sonrasında değişiklikleri foreach ile kaydederiz

            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new Entities.OrderItem()// iyzipay içerisinde de OrderItem var o yüzden Entities diyerek yolunu verdik
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                order.OrderItems.Add(orderItem);
            }
            _orderService.Create(order);// Kayıt yani sipariş veri tabanına aktarılır
        }
        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-uy88AF5idvnXEmDRzyrQHNINTIhsxK4Z";
            options.SecretKey = "sandbox-yEzhXVeSGd202ErGSfd1UdnnBr9iQk0i";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = new Random().Next(111111111, 999999999).ToString();// request içerisinde üretilen bir sayı, bu sayı ile return payment diyip buradan bir onay alınır ve tekrar payment üzerinden ConversationId bilgisine ulaşıp bu kaydın bize ait olup olmadığı kontrol etmek gerekir. Bu sayıyı daha farklı biçimlerlede üretebiliriz    
            request.Price = model.CartModel.TotalPrice().ToString();// Tüm ürünlerin toplam fiyatı
            request.PaidPrice = model.CartModel.TotalPrice().ToString();// kargo ücreti gibi başka ücretlerde eklersek eğer onların toplam fiyatı
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;// Taksit sayısı 
            request.BasketId = "B67832";// random sayı üretilebilir
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            // Cart bilgileri
            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            // Aşağıdaki kart bilgilerini uygulamada test amaçlı kullanabilirsin yana iyzipay sayfasındaki geliştirici sayfasından da kart bilgilerine ulaşabiliriz
            // paymentCard.CardNumber = "5528790000000008";
            // paymentCard.ExpireMonth = "12";
            // paymentCard.ExpireYear = "2030";
            // paymentCard.Cvc = "123";

            // Satın alan kişi bilgileri
            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = model.FirstName;
            buyer.Surname = model.LastName;
            buyer.GsmNumber = model.Phone;
            buyer.Email = model.Email;
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = model.Address;
            buyer.Ip = "85.34.78.112";
            buyer.City = model.City;
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            // Sepet içerisindeki ürün bilgileri. Tek tek yazmak yerine foreeach ile döndürürüz
            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in model.CartModel.CartItems)
            {
                basketItem = new BasketItem();// Yukarıda ilk başta bir nesne oluşturduk. Sonrasında her döngü döndüğünde yeni BasketItem oluştururuz
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "Telefon";
                basketItem.Price = item.Price.ToString();
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();// Bu alanı yazmazsak hata alırız
                basketItems.Add(basketItem);// Her seferinde oluşturduğumuz BasketItem'ı göndeririz yani ekleriz
            }
            request.BasketItems = basketItems;// İtemlar üzerine liste olarak ekleriz
            return Payment.Create(request, options);
        }
    }
}
