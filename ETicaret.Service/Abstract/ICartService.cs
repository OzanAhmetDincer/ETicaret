using ETicaret.Data.Abstract;
using ETicaret.Entities;

namespace ETicaret.Service.Abstract
{
    public interface ICartService : ICartRepository
    {
        void InitializeCart(string userId);
        Cart GetCartByUserId(string userId);
        void AddToCart(string userId, int productId, int quantity);
        void DeleteFromCart(string userId, int productId);
        void ClearCart(int cartId);
    }
}
