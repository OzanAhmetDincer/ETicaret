using ETicaret.Entities;

namespace ETicaret.Data.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetCategoryByProductsAsync(int id);
    }
}
