using ETicaret.Entities;

namespace ETicaret.Data.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllProductsByCategoriesBrandsAsync();
        Task<Product> GetProductByCategoriesBrandsAsync(int id);
    }
}
