using ETicaret.Data.Abstract;
using ETicaret.Entities;

namespace ETicaret.Service.Abstract
{
    public interface ICartService : ICartRepository
    {
        void InitializeCart(string userId);// Kartı başlatmak aktif hale getirmek için geri dönüş değeri olmayan metot tanımladık. Kullanıcının userId'sini alarak cartitem'ı cart tablosunda oluşturacaz
        Cart GetCartByUserId(string userId);// Kullanıcının cart'ını userId bilgisine göre almak için tanımladık
        void AddToCart(string userId, int productId, int quantity);// Her hangi bir ürünü sepete eklemek için tanımladık. Dışarıdan kullanıcının userId'si ni hangi ürün olduğunu anlamak için productId'sini ve ne kadar aldığı için quantity bilgilerini alırız.
        void DeleteFromCart(string userId, int productId);
        void ClearCart(int cartId);
    }
}
