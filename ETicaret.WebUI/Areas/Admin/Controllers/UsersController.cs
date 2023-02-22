using ETicaret.Entities;
using ETicaret.Service.Abstract;
using ETicaret.Service.Concrete;
using ETicaret.WebUI.Areas.Admin.Models;
using ETicaret.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

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
        }
    }
}
