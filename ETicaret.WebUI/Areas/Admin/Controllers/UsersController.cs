using ETicaret.Entities;
using ETicaret.Service.Abstract;
using ETicaret.Service.Concrete;
using ETicaret.WebUI.Areas.Admin.Models;
using ETicaret.WebUI.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<Identity.User> _userManager;
        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<Identity.User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: UsersController
        public IActionResult Index()
        {
            // _userMaanager üzerinden Users özelliği ile veri tabanındaki bütün kullanıcı bilgilerini getirir
            return View(_userManager.Users);
        }

        // GET: UsersController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);// Dışarıdan gönderdiğimiz id bilgisi ile _userManager üzerinden FindByIdAsync metodu ile kulanıcı bilgisini alırız
            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);// Kullanıcının role bilgilerini alırız
                var roles = _roleManager.Roles.Select(i => i.Name);// Veri tabanındaki tüm role bilgilerini alırız. _roleManager üzerinden Roles özelliği ile tüm role bilgileri üzerinde dolaşırız ve Select özelliği ile bu role bilgilerinin isimlerini alırız

                ViewBag.Roles = roles;// Tüm role bilgilerini ViewBag içerisinde sayfaya taşırız
                return View(new UserModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("/Admin/Users");
        }


        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    // Eğer kullanıcı null değilse veri tabanındaki user bilgisine model üzerinden gönderilen bilgiler aktarılır
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        // Kullanıcıya ait role bilgilerini _userManager üzerinden GetRolesAsync metodu ile alırız. Tüm role bilgilerini almak istesek rolemanager üzerinden alırız.
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };// selectedRoles içerisine eğer kullanıcıya farklı bir role tanımladıysak yani değişiklik varsa bu yeni role'yi selectedRoles'e aktarırız "??" ile de null kontrolü yaparız. Her hangi bir değişiklik yoksa boş bir string dizisi göndeririz.
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());// Daha önceden veri tabanında olmayan role seçmişse bunu karşılaştırmamız gerekiyor. _userManager üzerinden AddToRolesAsync metodu ile yeni seçilen role bilgilerini veri tabanına aktarırız. Bu metoda ilk parametre olarak değişiklik yapılacak user bilgisi verilir. Yukarıda bu user'i aldık. İkinci parametrede selectedRoles içerisinden yani formdan gönderilen bilgiler içerisinden Except metodu ile veri tabanında daha önce olan role bilgilerini çıkarırız, except "hariç" anlamına gelir yani formdan gönderilen bilgiler üzerinden örneği 3 tane seçtin ve bu 3 tane içerisinden 2 tanesi veri tabanında varsa bunları userRoles ile çıkarmış oluruz çıkardıktan sonra sonucu ToArray ile string dizisine çeviriyoruz ve elimizde kalan daha önceden olmayan role bilgilerini veri tabanına eklemiş oluyoruz. 
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());// Burada da userRoles yani veri tabanında kayıtlı olan bilgiler içerisinden selectedRoles'leri yani zaten varsa bu kayıtları biz userRoles içerisinden çıkarıp geri kalan kayıtları yani veri tabanından silinmesi gereken kayıtları RemoveFromRolesAsync ile veri tabanından ilgili kayıtlar çıkarılacak.

                        return Redirect("/Admin/Users");
                    }
                }
                return Redirect("/Admin/Users");
            }

            return View(model);

        }


        /*private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: UsersController
        public ActionResult Index()
        {
            var kullanici = new UserListModel()
            {
                Users = _userService.GetAll()
            };

            return View(kullanici);
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var kullanici = new User()
                    {
                        Name = user.Name,
                        SurName = user.SurName,
                        UserName = user.UserName,
                        Email = user.Email,
                        Phone = user.Phone,
                        Password = user.Password,
                        IsActive = user.IsActive,
                        IsAdmin = user.IsAdmin,
                        CreateDate = DateTime.Now
                    };
                    await _userService.AddAsync(kullanici);
                    await _userService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(user);
        }

        // GET: UsersController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var model = await _userService.FindAsync(id);
            var kullanici = new UserModel()
            {
                Id = model.Id,
                Name = model.Name,
                SurName = model.SurName,
                UserName = model.UserName,
                Email = model.Email,
                Phone = model.Phone,
                Password = model.Password,
                IsActive = model.IsActive,
                IsAdmin = model.IsAdmin,
                CreateDate = DateTime.Now
            };
            return View(kullanici);
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var model = new User()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        SurName = user.SurName,
                        UserName = user.UserName,
                        Email = user.Email,
                        Phone = user.Phone,
                        Password = user.Password,
                        IsActive = user.IsActive,
                        IsAdmin = user.IsAdmin,
                        CreateDate = DateTime.Now
                    };
                    _userService.Update(model);
                    await _userService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(user);
        }

        // GET: UsersController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _userService.FindAsync(id);
            var kullanici = new UserModel()
            {
                Id = model.Id,
                Name = model.Name,
                SurName = model.SurName,
                UserName = model.UserName,
                Email = model.Email,
                Phone = model.Phone,
                Password = model.Password,
                IsActive = model.IsActive,
                IsAdmin = model.IsAdmin,
                CreateDate = DateTime.Now
            };
            return View(kullanici);
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, User user)
        {
            try
            {
                _userService.Delete(user);
                await _userService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
