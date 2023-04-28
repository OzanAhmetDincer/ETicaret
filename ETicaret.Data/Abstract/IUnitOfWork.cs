namespace ETicaret.Data.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        // IUnitOfWork yapısını kullanmadığımız zaman biz her bir repository'i ayrı ayrı ele alıyoruz. Her bir repository üzerinden işlem yaptığımızda "context.SaveChanges()" çağırıyoruz. Buna gerek yok. Repository sınıflarının hepsini bir sınıf içerisinde gruplayarak kullanabilmek için UnitOfWork Pattern yapısını kullanırız.   
        IBrandRepository Brands { get; }
        ICartRepository Carts { get; }
        ICategoryRepository Categories { get; }
        IContactRepository Contacts { get; }
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        ISliderRepository Sliders { get; }
        void Save();
    }
}
