using ETicaret.Data.Abstract;
using ETicaret.Entities;

namespace ETicaret.Service.Abstract
{
    public interface IService<T> : IRepository<T> where T : class, IEntity, new()
    {
    }
}
