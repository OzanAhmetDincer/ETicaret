using ETicaret.Data;
using ETicaret.WebUI.Identity;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.WebUI.Extensions
{
    public static class MigrationManager
    {
        // Veri tabanında bir güncelleme yaptıktan sonra bir migration oluşturuyoruz. Bu migration'ları databese'ye aktarmamız gerekiyor. Her seferinde kendimiz aktarmaktan ziyade uygulama çalıştığı anda aktarılabilir. 
        //CreatHostBuilder metodu üzerinden çağırılan Build metodu bize IHost tipinde bir interface döndürüyor, biz bu interface'yi genişleterek bu interface üzerinden bir migrate metodu çalıştırabiliriz. [CreateHostBuilder(args).Build().MigrateDatabase().Run();] 
        public static IHost MigrateDatabase(this IHost host)// this parametresini extension metot kullanırken yazmamız gerekiyor.
        {
            // Program.cs içerisinde tanımladığımız servisleri alarak Migrate işlemini yaparız 
            using (var scope = host.Services.CreateScope())
            {
                using (var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
                {
                    try
                    {
                        applicationContext.Database.Migrate();// applicationContext'i alıp Database özelliği üzerinden Migrate metodu ile bekleyen migrationlar database'ye aktarılır
                    }
                    catch (Exception)
                    {
                        // loglama
                        throw;
                    }
                }

                using (var shopContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
                {
                    try
                    {
                        shopContext.Database.Migrate();
                    }
                    catch (Exception)
                    {
                        // loglama
                        throw;
                    }
                }
            }

            return host;
        }
    }
}
