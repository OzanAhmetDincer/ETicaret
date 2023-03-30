using ETicaret.Entities;
using ETicaret.Service.Abstract;
using ETicaret.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class ContactsController : Controller
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        // GET: ContactsController
        public ActionResult Index()
        {
            var iletisim = new ContactListModel()
            {
                Contacts = _contactService.GetAll()
            };

            return View(iletisim);
        }

        // GET: ContactsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContactsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(ContactModel contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var iletisim = new Contact()
                    {
                        Name = contact.Name,
                        SurName = contact.SurName,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Message = contact.Message,
                        CreateDate = DateTime.Now
                    };
                    await _contactService.AddAsync(iletisim);
                    await _contactService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(contact);
        }

        // GET: ContactsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var model = await _contactService.FindAsync(id);
            var iletisim = new ContactModel()
            {
                Id = model.Id,
                Name = model.Name,
                SurName = model.SurName,
                Email = model.Email,
                Phone = model.Phone,
                Message = model.Message,
                CreateDate = DateTime.Now
            };
            return View(iletisim);
        }

        // POST: ContactsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(ContactModel contacts)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var model = await _contactService.FindAsync(contacts.Id);
                    model.Name = contacts.Name;
                    model.SurName = contacts.SurName;
                    model.Email = contacts.Email;
                    model.Phone = contacts.Phone;
                    model.Message = contacts.Message;
                    model.CreateDate = DateTime.Now;
                    _contactService.Update(model);
                    await _contactService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(contacts);
        }

        // GET: ContactsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _contactService.FindAsync(id);
            var iletisim = new ContactModel()
            {
                Id = model.Id,
                Name = model.Name,
                SurName = model.SurName,
                Email = model.Email,
                Phone = model.Phone,
                Message = model.Message,
                CreateDate = DateTime.Now
            };
            return View(iletisim);
        }

        // POST: ContactsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Contact contact)
        {
            try
            {
                _contactService.Delete(contact);
                await _contactService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
