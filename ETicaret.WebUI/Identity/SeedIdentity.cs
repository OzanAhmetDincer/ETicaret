﻿using ETicaret.Service.Abstract;
using Microsoft.AspNetCore.Identity;

namespace ETicaret.WebUI.Identity
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ICartService cartService, IConfiguration configuration)
        {
            // Projeyi canlıya alacağımız zaman admin sayfasına giriş yapabilmek ve işlemleri yapabilecek bir kullanıcıyı eklememiz gerekir. Çünkü proje canlıya alındığında veri tabanında kayıtlı herhangi bir kullanıcı olmayacak
            // "IConfiguration configuration" yapısını configürasyon işlemlerini yapabilmek için ekledik. Yani appsettings.json dosyası içerisine yazdığımız user bilgileri üzerinde işlem yapabilmek için ekledik. Configuration üzerinden appsettings'e ulaşıp o bilgileri oradan alıcaz.  
            
            // Hemen aşağıdaki işlemlerlede kullanıcı oluştururuz fakat bu şekilde oluşturulan kullanıcıların bir cart objesi olmadığından dolayı cart bilgilerini göremez. Normalde kayıt olan bir kullanıcının hemen cart bilgisi oluşur.
            /*var username = configuration["Data:AdminUser:username"];// appsettings.json dosyası içerisindeki Data altındaki AdminUser altındaki username'yi burada tanımlarız.
            var email = configuration["Data:AdminUser:email"];
            var password = configuration["Data:AdminUser:password"];
            var role = configuration["Data:AdminUser:role"];

            if (await userManager.FindByNameAsync(username)==null)// username ile alakalı bir kullanıcı var mı ona bakarız. Eğer null ise aşağıdaki kullanıcı oluşturma işlemlerini yaparız.
            {
                await roleManager.CreateAsync(new IdentityRole(role));// ilk başta bir role bilgisi oluştururuz. Role bilgisi içerisine ise yukarıda ayarladığımız role bilgisini veririz ve oluşur.
                var user = new User()
                {
                    UserName = username,
                    Email = email,
                    FirstName = "admin",
                    LastName = "admin",
                    EmailConfirmed = true,
                };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }*/



            var roles = configuration.GetSection("Data:Roles").GetChildren().Select(x => x.Value).ToArray();// roles bilgilerini alırız. configuration objesi aracılığı ile appsettings.json dosyasına ulaşıcaz GetSection ile appsettings dosyası altındaki Sectionlara ulaşırız. Biz burada Data altındaki Roles section'una ulaştık. Sonrasında GetChildren ile alt elemanlarını alırız. Select ile de alt elemanları üzerinde dolaşırız yani seçebiliriz. "x => x.Value" ile ulaştığımız her elemanın değerini alırız. Seçme yaptıktan sonra bunu bir döngü yani liste olarak ToArray ile çeviririz. configuration'dan okumuş olduğumuz bir değer string olarak gelir. Bunu biz listeye çevirmemiz gerekir.   

            foreach (var role in roles)// Roles bilgilerini döngüye aldık
            {
                if (!await roleManager.RoleExistsAsync(role))// Eğer Roles bilgisi ekli değilse yeni Roles bilgisini ekleriz.
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var users = configuration.GetSection("Data:Users");// appsettings.json altındaki Data altındaki Users bilgilerini aldık.

            foreach (var section in users.GetChildren())// Users bilgileri ve içerisindeki değerler üzerinde tek tek gezeriz. Aşağıda bu bilgilere erişiriz 
            {
                var username = section.GetValue<string>("username");
                var password = section.GetValue<string>("password");
                var email = section.GetValue<string>("email");
                var role = section.GetValue<string>("role");
                var firstName = section.GetValue<string>("firstName");
                var lastName = section.GetValue<string>("lastName");

                if (await userManager.FindByNameAsync(username) == null)// Eğer kayıtlı olan herhangi bir kullanıcı ile eşleşip eşleşmediğine bakarız eğer yoksa yukarıda ulaştığımız bilgileri aşağıda doldururuz.
                {
                    var user = new User()
                    {
                        UserName = username,
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);// Kullanıcı ilgili role eklendiğinde InitializeCart ile cart objesi oluşur 
                        cartService.InitializeCart(user.Id);
                    }
                }

            }



        }
    }
}
