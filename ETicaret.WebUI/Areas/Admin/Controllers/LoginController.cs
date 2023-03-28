using ETicaret.Service.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ETicaret.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using ETicaret.WebUI.Identity;
using ETicaret.WebUI.Extensions;
using ETicaret.WebUI.Models;

namespace ETicaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private SignInManager<User> _signInManager;// "SignInManager" Microsoft Identity içerisinde olan bir yapı. Bu da cookie olaylarını yönetecek
        public LoginController(IUserService userService, SignInManager<User> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Index(string email, string password)
        //{
        //    try
        //    {
        //        var kullanici = await _userService.FirstOrDefaultAsync(k => k.IsActive && k.Email == email && k.Password == password);
        //        if (kullanici != null)
        //        {
        //            var kullaniciHaklari = new List<Claim>()
        //            {
        //                new Claim(ClaimTypes.Name, kullanici.Name),
        //                new Claim("Role", kullanici.IsAdmin ? "Admin" : "User"),
        //                new Claim("UserTd", kullanici.Id.ToString())
        //            };
        //            var kullaniciKimliği = new ClaimsIdentity(kullaniciHaklari, CookieAuthenticationDefaults.AuthenticationScheme);//yada (kullaniciHaklari, "Login") yazarakta admin giriş sayfasından giriş yaptırabiliriz.
        //            ClaimsPrincipal principal = new(kullaniciKimliği);
        //            await HttpContext.SignInAsync(principal);
        //            return Redirect("/Admin/Main");
        //        }
        //        else TempData["Mesaj"] = "Giriş Başarısız!";
        //    }
        //    catch (Exception hata)
        //    {
        //        //hata.Message
        //        // todo(yapılacaklar): hatalar db ye loglanacak
        //        TempData["Mesaj"] = "Hata Oluştu!";
        //    }
        //    return View();
        //}

        //[Route("/Admin/Logout")]// eğer uygulamada Admin/Logout url adresine istek gelirse, normalde adresi Admin/Login/Logout olan aşağıdaki metodu çalıştırır
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();// "SignOutAsync" metodu cookie'yi tarayıcıdan siler ve cookie olmadığı için tekrardan login olmamız gerekir.
        //    TempData.Put("message", new AlertMessage()
        //    {
        //        Title = "Oturum Kapatıldı.",
        //        Message = "Hesabınız güvenli bir şekilde kapatıldı.",
        //        AlertType = "warning"
        //    });
        //    return Redirect("~/");
        //}

        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Register(LoginAdminModel model)
        //{
        //    return View();
        //}
    }
}
