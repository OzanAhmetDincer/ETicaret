using ETicaret.Data.Abstract;
using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Data.Concrete
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(DatabaseContext _context) : base(_context)
        {
        }

        public void ClearCart(int cartId)
        {
            var cmd = @"delete from CartItems where CartId=@p0"; 
            context.Database.ExecuteSqlRaw(cmd,cartId);
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            var cmd = @"delete from CartItems where CartId=@p0 and ProductId=@p1";
            context.Database.ExecuteSqlRaw(cmd, cartId, productId);
        }

        public Cart GetByUserId(string userId)
        {
            return context.Carts.Include(i => i.CartItems).ThenInclude(i => i.Product).FirstOrDefault(i => i.UserId == userId);
        }
        public override void Update(Cart entity)
        {
            context.Carts.Update(entity);
        }
    }
}
