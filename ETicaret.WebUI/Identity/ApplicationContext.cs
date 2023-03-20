using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.WebUI.Identity
{
    // Veritabanı ile iletişimi sağlayacak olan entityframeworkcore mantığı gibi bir context'i oluştururuz. Bu class bu sefer DbConntext'in özelleştirimiş bir hali olan IdentityDbContext' den türeticez ve bu contex bizim tanımlamış olduğumuz "User" ile çalışacak.
    public class ApplicationContext : IdentityDbContext<User>
    {
        // constractor metodu ile dışarıdan bazı parametreler göndericez.  
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) // ilk tanımlamış olduğumuz "options" ı base "options" a göndeririz.
        {

        }
    }
}
