using System.Linq.Expressions;

namespace ETicaret.Data.Abstract
{
    public interface IRepository<T> where T : class// IRepository interface i Entity lerimiz için gerçekleştireceğimiz veritabanı işlemleriniyapacak olan Repository class ında bulunması gereken metotların imzalarını tutuyor. <T> kodu, bu interface e dışarıdan parametre olarak generic bir nesnesinin gönderilebilmesini sağlar. where T : class kodu ise T nin tipinin class olması gerektiğini belirler, böylece string gibi bir veri gönderilmeye kalkılırsa interface bunu kabul etmeyecektir.
    {
        List<T> GetAll();// veritabanındaki tüm kayıtları listeleyecek metot imzası
        List<T> GetAll(Expression<Func<T, bool>> expressions);// GetAll metodunda entity frameworktaki x=>x. şeklinde yazdığımızz lambda expression larını kullanabilmek için
        T Get(Expression<Func<T, bool>> expressions);//özel sorgu kullanarak 1 tane kayıt getiren metot imzası
        T Find(int id);
        int Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int SaveChanges();
        //Asenkron Metotlar
        Task<T> FindAsync(int id);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expressions);
        IQueryable<T> FindAllAsync(Expression<Func<T, bool>> expressions);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expressions);
        Task AddAsync(T entity);
        Task<int> SaveChangesAsync();
    }
}
