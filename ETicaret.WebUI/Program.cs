using ETicaret.Data;
using ETicaret.Data.Abstract;
using ETicaret.Data.Concrete;
using ETicaret.Service.Abstract;
using ETicaret.Service.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));

builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddTransient<IContactRepository, ContactRepository>();
builder.Services.AddTransient<ISliderRepository, SliderRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<IContactService, ContactService>();
builder.Services.AddTransient<ISliderService, SliderService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();// IHttpContextAccessor ile uygulama içerisindeki giriþ yapan kullanýcý, session verileri, cookie ler gibi içeriklere view lardan veya controllerdan ulaþabilmesini saðlar.

// Authentication: oturum açma servisi
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login"; // giriþ yapma sayfasý
    x.AccessDeniedPath = "/AccessDenied";// giriþ yapan kullanýcýnýn admin yetkisi yoksa AccessDenied sayfasýna yönlendir
    x.LogoutPath = "/Admin/Login/Logout";// çýkýþ sayfasý 
    x.Cookie.Name = "Administrator";// oluþacak kukinin adý
    x.Cookie.MaxAge = TimeSpan.FromDays(1);// oluþacak kukinin yaþam süresi
});


// Authorization : Yetkilendirme
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", policy => policy.RequireClaim("Role", "Admin"));
    x.AddPolicy("UserPolicy", policy => policy.RequireClaim("Role", "User"));
});

var app = builder.Build();

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

app.Run();
