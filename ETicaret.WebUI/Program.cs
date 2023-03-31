using ETicaret.Data;
using ETicaret.Data.Abstract;
using ETicaret.Data.Concrete;
using ETicaret.Service.Abstract;
using ETicaret.Service.Concrete;
using ETicaret.WebUI.EmailServices;
using ETicaret.WebUI.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>();// Entityframework i�lemlerini yapabilmek i�in bu sat�r� ekliyoruz. Veritaban� yap�land�rmas�n� yapm�� olduk.
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection")));//ApplicationContext i�in connection string ba�lamas� yapt�k. "Configuration" ile "appsettings.json" dosyas�na ula��r�z. "GetConnectionString" ile appsettings i�erisindeki "ConnectionStrings" alt�ndaki verilere ula��r�z ve orada hangi connection string'i kullanacaksak onun ismini veririz.(SqlConnection).
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();// Olu�turdu�umuz Identity'i projeye dahil etmemiz gerekiyor. Identity kls�r�nde tan�mlam�� oldu�umuz "User"(kullanaca��m�z kullan�c� bilgisi) ve "IdentityRole"(Kullanaca��m�z role tablolar� i�in s�n�f� yazar�z. Bunu "IdentityRole" den t�reyen bir class olarak olu�turabilirsinde, User'i "IdentityUser" class'�ndan t�retti�imiz gibi. Biz burada temel s�n�f� kulland�k.) tablolar�n� veriyoruz, "AddEntityFrameworkStores" ile kullanaca��m�z veritaban�n� tan�ml�yoruz. "AddDefaultTokenProviders" ile tokenprovider eklememizdeki sebep sadece bir parola resetleme i�lemleri gibi konularda bu benzersiz bir say� �retir, onu mail olarak kullan�c�ya g�ndeririz ve o benzersiz say� ile beraber de�i�iklik i�lemini ger�ekle�tirir. Bu gibi i�lemleri yapmam�z� sa�layan benzersiz bir say� �retir.  
builder.Services.Configure<IdentityOptions>(options =>
{
    // password
    options.Password.RequireDigit = true;// �ifrede mutlaka say�sal bir de�er girmek zorunda true oldu�u i�in.
    options.Password.RequireLowercase = true;// �ifrede mutlaka k���k harf olmak zorunda
    options.Password.RequireUppercase = true;// �ifrede mutlaka b�y�k harf olmak zorunda
    options.Password.RequiredLength = 6;// �ifre min 6 karakter olmak zorunda
    options.Password.RequireNonAlphanumeric = true;// @, _ gibi i�aretler olmak zorunda

    // lockout(kullan�c�n�n hesab�n�n kilitlenip kilitlenmemesi ile ilgili)
    options.Lockout.MaxFailedAccessAttempts = 5;// Kullan�c� max 5 kez yanl�� bilgi girebilir, sonras�nda hesap kitlenir
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);// 5 dk kilitli kald�ktan sonra giri� yapmaya devam eder
    options.Lockout.AllowedForNewUsers = true;// Tekrardan giri� yapmas�na izin vermek i�in bunu aktif etmeliyiz.


    //options.User.AllowedUserNameCharacters = "";(kullan�c� ad� i�erisinde kullan�lmas�n� yada kullan�lmamas�n� istedi�iniz karakter tan�mlamas� yap�l�r)
    options.User.RequireUniqueEmail = true;// Her kullan�c�n�n bir birinden farkl� email adresinin olmas� gerekiyor. Ayn� mail adresi ile iki kullan�c� olamaz

    options.SignIn.RequireConfirmedEmail = true;// Kullan�c� �ye olduktan sonra mutlaka hesab�n� onaylamas� laz�m. Onay mailinden onaylama yap�lmas� laz�m
    options.SignIn.RequireConfirmedPhoneNumber = false;// Kullan�c� �ye olduktan sonra verdi�i telefon �zerinden onay olmas� gerekmez, "true" olursa gerekiyor
});

// A�a��da olu�turulacak olan cookie'yi tan�mlayacaz. "cookie" kullan�c�n�n taray�c�s�nda uygulama taraf�ndan b�rak�lan bir bilgidir. Biz bir uygulamaya girdi�imiz de server taraf�ndan bize belirli bilgiler gelir ve taray�c�m�za b�rak�rki daha sonra biz bu uygulamay� ziyaret etti�imizde server taraf� bizi tan�r.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";// Ben cookie session taraf�ndan tan�nm�yorsam(cookie'nin s�resi bitmi�se vb) uygulama bizi "account" controller alt�nda ki "login" sayfas�na g�nderecek
    options.LogoutPath = "/account/logout";// Kullan�c� ��k��� yapmak istedi�im zaman cookie taray�c�dan silinecek. Cookie ve session aras�nda bir ba�lant� kalmayacak ve bizi "account" controller alt�nda ki "logout sayfas�na g�nderir"
    options.AccessDeniedPath = "/account/accessdenied";// Giri� yapan kullan�c�lar�n her sayfaya giri� yapamamas� gerekir yani yetkisinin olmamas� gerekir. Kullan�c� yetkisinin olmad��� bir alana girdi�i zaman bizi uyaracak olan bir sayfaya y�nlendirir. bizi "account" controller alt�nda ki "accessdenied sayfas�na g�nderir"
    options.SlidingExpiration = true;// Taray�c� varsay�lan� olarak cookie s�resi 20dk'd�r. Bu s�reden sonra cookie silinir. "SlidingExpiration" true dedi�imiz zaman bu 20dk s�reyi her istek yapt���m�zda s�f�rlar. Yani 19. dakikada cookie biti� s�resi ve sen bir istek yapt�k bu s�reyi tekrar 20dk olarak ba�lat�r.
    options.ExpireTimeSpan = TimeSpan.FromDays(30);// Bu varsay�lan olarak tan�mlanan 20dk olan s�reyi de�i�tirebiliriz. Cookie'nin ya�am s�resini belirledik.
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,// Cookie'yi sadece bir http talebi ile elde ederiz. Yani herhangi bir js yap�s� bizim cookie'ye ula�amas�n, sadece http ile alabilir
        Name = ".ETicaret.Security.Cookie",// Taray�c�da olu�acak olan cookie'nin ismi
        SameSite = SameSiteMode.Strict// Bize ait olan bir cookie'ye ba�ka biri sahip olsa bile bize ait olan adresten bir istek gitmesi gerekiyor. Bu kod bu i�e yar�yor.
    };
});

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));

builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddTransient<IContactRepository, ContactRepository>();
builder.Services.AddTransient<ISliderRepository, SliderRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<IContactService, ContactService>();
builder.Services.AddTransient<ISliderService, SliderService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();// IHttpContextAccessor ile uygulama i�erisindeki giri� yapan kullan�c�, session verileri, cookie ler gibi i�eriklere view lardan veya controllerdan ula�abilmesini sa�lar.

// Ekledi�imiz EmailServisini burada �a��r�r�z. Bu service bizden SmtpEmailSender i�erisindeki contractor alt�ndaki parametreleri g�ndermemiz gerekir. SmtpEmailSender'� �a��rd���m�z zaman bir nesne �retilecek ve nesne �retilirkende i�erisindeki parametreleri a�a��daki gibi g�ndermemiz gerekiyor. Bu parametreleride appsettings i�erisinden ald�k.
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>(i => new SmtpEmailSender(
    builder.Configuration["EmailSender:Host"],
    builder.Configuration.GetValue<int>("EmailSender:Port"),
    builder.Configuration.GetValue<bool>("EmailSender:EnableSSL"),
    builder.Configuration["EmailSender:UserName"],
    builder.Configuration["EmailSender:Password"]
));

var app = builder.Build();

//IConfiguration configuration = app.Configuration;
//UserManager<User> userManager = app.;
//RoleManager<IdentityRole> roleManager;
//ICartService cartService = app.Services.GetRequiredService<ICartService>();
//SeedIdentity.Seed(userManager, roleManager, cartService, configuration).Wait();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Authentication: oturum a�ma-giri� yapma
app.UseAuthentication(); // admin login i�in. UseAuthentication �n UseAuthorization dan �nce gelmesi zorunlu! 
//Authorization: yetkilendirme (oturum a�an kullan�c�n�n admine giri� yetkisi var m�?)
app.UseAuthorization();


app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "custom",
    pattern: "{customurl?}/{controller=Home}/{action=Index}/{id?}");


var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    /*var cartService = scope.ServiceProvider.GetRequiredService<ICartService>();*/
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
     SeedIdentity.Seed(userManager,roleManager,/*cartService*/configuration).Wait();// Asenkron oldu�u i�in sonuna "Wait" ekledik
}
app.Run();
