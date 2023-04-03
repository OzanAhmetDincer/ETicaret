using ETicaret.Data;
using ETicaret.Data.Abstract;
using ETicaret.Data.Concrete;
using ETicaret.Entities;
using ETicaret.Service.Abstract;

namespace ETicaret.Service.Concrete
{
    public class CartService : CartRepository, ICartService
    {
        // Veri tabanı ile iletişim sağlamak için CartRepository'den değil yani class'dan değil interface'i(ICartRepository) kullanacak ve dependency injection işlemi yaparız.
        private ICartRepository _cartRepository;
        public CartService(DatabaseContext _context, ICartRepository cartRepository) : base(_context)
        {
            _cartRepository = cartRepository;
        }
        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);// Aşağıda ki GetCartByUserId metodu ile login olan kullanıcının cart bilgisini almıştık burada onu kullanabiliriz.
            if (cart != null)
            {
                // eklenmek isteyen ürün sepette varmı (güncelleme)
                // eklenmek isteyen ürün sepette var ve yeni kayıt oluştur. (kayıt ekleme)
                // Burada iki ihtimal var. İlk olarak kullanıcının cart'ı(alışveriş sepeti) içerisinde ürün vardır ve kullanıcı sadce bu ürünün miktarında değişiklik yapmak istiyordur. Bu durumda sıfırdan bir cart oluşturmaya gerek yok. Direkt o ürün bilgilerini güncellememiz gerek. O yüzden hemen aşağıdaki tanımladığımız index değişkenine şu tanımlanır; cart içindeki CartItems'a ulaş ve FindIndex metodu ile içerisinde eklenecek yada değişiklik yapılacak ürün varmı onu buluruz.
                var index = cart.CartItems.FindIndex(i => i.ProductId == productId);
                if (index < 0)
                {
                    // Eğer dışarıdan gönderdiğimiz productId ile veri tabanında eşleşen ProductId yoksa yani o üründen cart içerisinde bulunmuyorsa yeni bir CartItem ekleyecez. İçerisine dışardan aldığımız değerleri göndeririz.
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        CartId = cart.Id
                    });
                }
                else
                {
                    // Liste içerisinde eklemek istediğimiz ürün varsa yani index>0 ise cart içerisindeki CartItem'a index bilgisini vererek hangi üründe değişiklik yapılacağını belirtiriz ve kayırlı olan Quantity bilgisi ile dışarıdan gönderdiğimiz quantity bilgisini toplarız.
                    cart.CartItems[index].Quantity += quantity;
                }

                _cartRepository.Update(cart);
                _cartRepository.SaveChanges();

            }
        }

        public void ClearCart(int cartId)
        {
            _cartRepository.ClearCart(cartId);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                _cartRepository.DeleteFromCart(cart.Id, productId);
            }
        }

        public Cart GetCartByUserId(string userId)
        {
            return _cartRepository.GetByUserId(userId);
        }

        public void InitializeCart(string userId)
        {
            // _cartRepository.Create(new Cart(){UserId = userId});
            _cartRepository.Add(new Cart() { UserId = userId });
            _cartRepository.SaveChanges();
        }
    }
}
