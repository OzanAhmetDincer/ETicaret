using ETicaret.WebUI.Extensions;
using ETicaret.WebUI.Identity;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles ="Admin")]
    public class MainController : Controller
    {
        private SignInManager<User> _signInManager;// "SignInManager" Microsoft Identity içerisinde olan bir yapı. Bu da cookie olaylarını yönetecek

        public MainController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/Admin/Logout")]// eğer uygulamada Admin/Logout url adresine istek gelirse, normalde adresi Admin/Main/Logout olan aşağıdaki metodu çalıştırır
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();// "SignOutAsync" metodu cookie'yi tarayıcıdan siler ve cookie olmadığı için tekrardan login olmamız gerekir.
            TempData.Put("message", new AlertMessage()
            {
                Title = "Oturum Kapatıldı.",
                Message = "Hesabınız güvenli bir şekilde kapatıldı.",
                AlertType = "warning"
            });
            return Redirect("~/");
        }
    }
}
