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
builder.Services.AddDbContext<DatabaseContext>();// Entityframework iþlemlerini yapabilmek için bu satýrý ekliyoruz. Veritabaný yapýlandýrmasýný yapmýþ olduk.
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection")));//ApplicationContext için connection string baðlamasý yaptýk. "Configuration" ile "appsettings.json" dosyasýna ulaþýrýz. "GetConnectionString" ile appsettings içerisindeki "ConnectionStrings" altýndaki verilere ulaþýrýz ve orada hangi connection string'i kullanacaksak onun ismini veririz.(SqlConnection).
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();// Oluþturduðumuz Identity'i projeye dahil etmemiz gerekiyor. Identity klsöründe tanýmlamýþ olduðumuz "User"(kullanacaðýmýz kullanýcý bilgisi) ve "IdentityRole"(Kullanacaðýmýz role tablolarý için sýnýfý yazarýz. Bunu "IdentityRole" den türeyen bir class olarak oluþturabilirsinde, User'i "IdentityUser" class'ýndan türettiðimiz gibi. Biz burada temel sýnýfý kullandýk.) tablolarýný veriyoruz, "AddEntityFrameworkStores" ile kullanacaðýmýz veritabanýný tanýmlýyoruz. "AddDefaultTokenProviders" ile tokenprovider eklememizdeki sebep sadece bir parola resetleme iþlemleri gibi konularda bu benzersiz bir sayý üretir, onu mail olarak kullanýcýya göndeririz ve o benzersiz sayý ile beraber deðiþiklik iþlemini gerçekleþtirir. Bu gibi iþlemleri yapmamýzý saðlayan benzersiz bir sayý üretir.  
builder.Services.Configure<IdentityOptions>(options =>
{
    // password
    options.Password.RequireDigit = true;// Þifrede mutlaka sayýsal bir deðer girmek zorunda true olduðu için.
    options.Password.RequireLowercase = true;// Þifrede mutlaka küçük harf olmak zorunda
    options.Password.RequireUppercase = true;// Þifrede mutlaka büyük harf olmak zorunda
    options.Password.RequiredLength = 6;// Þifre min 6 karakter olmak zorunda
    options.Password.RequireNonAlphanumeric = true;// @, _ gibi iþaretler olmak zorunda

    // lockout(kullanýcýnýn hesabýnýn kilitlenip kilitlenmemesi ile ilgili)
    options.Lockout.MaxFailedAccessAttempts = 5;// Kullanýcý max 5 kez yanlýþ bilgi girebilir, sonrasýnda hesap kitlenir
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);// 5 dk kilitli kaldýktan sonra giriþ yapmaya devam eder
    options.Lockout.AllowedForNewUsers = true;// Tekrardan giriþ yapmasýna izin vermek için bunu aktif etmeliyiz.


    //options.User.AllowedUserNameCharacters = "";(kullanýcý adý içerisinde kullanýlmasýný yada kullanýlmamasýný istediðiniz karakter tanýmlamasý yapýlýr)
    options.User.RequireUniqueEmail = true;// Her kullanýcýnýn bir birinden farklý email adresinin olmasý gerekiyor. Ayný mail adresi ile iki kullanýcý olamaz

    options.SignIn.RequireConfirmedEmail = true;// Kullanýcý üye olduktan sonra mutlaka hesabýný onaylamasý lazým. Onay mailinden onaylama yapýlmasý lazým
    options.SignIn.RequireConfirmedPhoneNumber = false;// Kullanýcý üye olduktan sonra verdiði telefon üzerinden onay olmasý gerekmez, "true" olursa gerekiyor
});

// Aþaðýda oluþturulacak olan cookie'yi tanýmlayacaz. "cookie" kullanýcýnýn tarayýcýsýnda uygulama tarafýndan býrakýlan bir bilgidir. Biz bir uygulamaya girdiðimiz de server tarafýndan bize belirli bilgiler gelir ve tarayýcýmýza býrakýrki daha sonra biz bu uygulamayý ziyaret ettiðimizde server tarafý bizi tanýr.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";// Ben cookie session tarafýndan tanýnmýyorsam(cookie'nin süresi bitmiþse vb) uygulama bizi "account" controller altýnda ki "login" sayfasýna gönderecek
    options.LogoutPath = "/account/logout";// Kullanýcý çýkýþý yapmak istediðim zaman cookie tarayýcýdan silinecek. Cookie ve session arasýnda bir baðlantý kalmayacak ve bizi "account" controller altýnda ki "logout sayfasýna gönderir"
    options.AccessDeniedPath = "/account/accessdenied";// Giriþ yapan kullanýcýlarýn her sayfaya giriþ yapamamasý gerekir yani yetkisinin olmamasý gerekir. Kullanýcý yetkisinin olmadýðý bir alana girdiði zaman bizi uyaracak olan bir sayfaya yönlendirir. bizi "account" controller altýnda ki "accessdenied sayfasýna gönderir"
    options.SlidingExpiration = true;// Tarayýcý varsayýlaný olarak cookie süresi 20dk'dýr. Bu süreden sonra cookie silinir. "SlidingExpiration" true dediðimiz zaman bu 20dk süreyi her istek yaptýðýmýzda sýfýrlar. Yani 19. dakikada cookie bitiþ süresi ve sen bir istek yaptýk bu süreyi tekrar 20dk olarak baþlatýr.
    options.ExpireTimeSpan = TimeSpan.FromDays(30);// Bu varsayýlan olarak tanýmlanan 20dk olan süreyi deðiþtirebiliriz. Cookie'nin yaþam süresini belirledik.
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,// Cookie'yi sadece bir http talebi ile elde ederiz. Yani herhangi bir js yapýsý bizim cookie'ye ulaþamasýn, sadece http ile alabilir
        Name = ".ETicaret.Security.Cookie",// Tarayýcýda oluþacak olan cookie'nin ismi
        SameSite = SameSiteMode.Strict// Bize ait olan bir cookie'ye baþka biri sahip olsa bile bize ait olan adresten bir istek gitmesi gerekiyor. Bu kod bu iþe yarýyor.
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

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();// IHttpContextAccessor ile uygulama içerisindeki giriþ yapan kullanýcý, session verileri, cookie ler gibi içeriklere view lardan veya controllerdan ulaþabilmesini saðlar.

// Eklediðimiz EmailServisini burada çaðýrýrýz. Bu service bizden SmtpEmailSender içerisindeki contractor altýndaki parametreleri göndermemiz gerekir. SmtpEmailSender'ý çaðýrdýðýmýz zaman bir nesne üretilecek ve nesne üretilirkende içerisindeki parametreleri aþaðýdaki gibi göndermemiz gerekiyor. Bu parametreleride appsettings içerisinden aldýk.
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

//Authentication: oturum açma-giriþ yapma
app.UseAuthentication(); // admin login için. UseAuthentication ýn UseAuthorization dan önce gelmesi zorunlu! 
//Authorization: yetkilendirme (oturum açan kullanýcýnýn admine giriþ yetkisi var mý?)
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
     SeedIdentity.Seed(userManager,roleManager,/*cartService*/configuration).Wait();// Asenkron olduðu için sonuna "Wait" ekledik
}
app.Run();
