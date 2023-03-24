using ETicaret.WebUI.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Controllers
{
    public class RolesController : Controller
    {
        //RoleManager'i de User'da olduğu gibi Identity klasörü içindeki gibi genişletip kullanabilirsin. Biz şimdi tüm işlemleri "IdentityRole" üzerinden yapıcaz.
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
