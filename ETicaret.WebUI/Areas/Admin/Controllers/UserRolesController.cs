using ETicaret.WebUI.Areas.Admin.Models;
using ETicaret.WebUI.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles ="Admin")]
    public class UserRolesController : Controller
    {
        //RoleManager'i de User'da olduğu gibi Identity klasörü içindeki gibi genişletip kullanabilirsin. Biz şimdi tüm işlemleri temel sınıf olan "IdentityRole" üzerinden yapıcaz.
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public UserRolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        //[AllowAnonymous] => Bu özellik ile admin olsun olmasın üye olsun yada olmasın herkes login olmadan RoleList'i gerebilir. Url'den Admin/UserRoles/RoleList ile gider yada a etiketi ile yönlendirme yaparakta herkese açık yapılabilir. 
        public IActionResult RoleList()
        {
            // IdentityRole'den türettiğimiz _roleManager üzerinden tüm rolleri listeleriz.
            return View(_roleManager.Roles);
        }

        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));// _roleManager üzerinden CreateAsync metodu ile bir role oluştururuz. Bu metot bizden bir role ismi ister. Onu da RoleModel içinde tanımladığımız name'i veririz.
                if (result.Succeeded)
                {
                    // Oluşturme başarılır ise "RoleList" sayfasına yönlendirilir.
                    return RedirectToAction("RoleList");
                }
                else
                {
                    // Oluşturma da her hangi bir hata olursa foreach ile bunları ekrana yazdırırız.
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }


        public async Task<IActionResult> RoleEdit(string id)
        {
            // Gelen roleId'sine(string id) göre veri tabanında ilgili rolü sorgulayacaz ve o role ait olan veya olmayan kullanıcıları listeleyip bu kullanıcılar üzerinde işlemler yapılacak.
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();
            var nonmembers = new List<User>();

            // Aşağıdaki foreach döngüsü ile _userManager üzerinden "Users" özelliği ile veri tabanındaki tüm kullanıcılara ulaşırız ve tolist() ile liste oluşturup bu liste içinde geziniriz
            foreach (var user in _userManager.Users.ToList())
            {
                // IsInRoleAsync metodu foreach döngüsü ile tüm kullanıcıları dolaşırken seçtiğimiz role ait olup olmadığını kontrol eder 
                // Aşağıda ki kodda da kullanıcı eğer ilgili role ait ise geriye true değeri dönecek ve ternary operator'ü(?) aracılığıyla members listesini tanımladığımız "list" üzerine atıyoruz yani members'ın referansını list'e vermiş oluyoruz, değilsede nonmembers'ı vermiş oluyoruz. "list.Add(user)" ile eğer list değişkeninin tipi members döndüyse members listesinin üzerine ekleme yapar, nonmembers döndüyse nonmembers listesine ekleme yapar.
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonmembers;
                list.Add(user);
            }
            // Elde ettiğimiz bilgileri RoleDetails model üzerinden ekrana aktarırız.
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                // foreach ile seçilen role eklenecek olan kullanıcılar üzerinde geziniriz. Eğer null bir değer gelirse string bir dizi atayalım ki hata vermesin. Boş bir dizede foreach boş döner.
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    // FindByIdAsync ile eklenecek olan kullanıcıları userId ile alırız. Eklenecek kişilere NonMembers foreach'i altındaki "<input type="checkbox" name="IdsToAdd" value="@user.Id">" içerisindeki name bilgisi ile eşleştirip value ile kullanıcılara ulaşırız.
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        // Eğer kullanıcı varsa(user null' eşit değilse) _userManager üzerinden AddToRoleAsync metodu ile ekleme yaparız. İlk parametre kullanıcı bilgisi, ikinci parametrede bizden rolename bekler. İlgili RoleName'ye ilgili kullanıcıyı burada atarız.    
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            // Eklenme işleminde her hangi bir hata varsa ekrana yazdırırız.
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                // Yukarıda yapılan işlem mantığı ile aşağıda silme işleminide yaparız.
                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            // Ekleme yada silme işlemi yapıldıktan sonra tekrardan aynı role bilgilerini içeren ekleme ve silme sayfasına yönlendiririz.
            return Redirect("/Admin/UserRoles/RoleEdit/" + model.RoleId);
        }
    }
}
