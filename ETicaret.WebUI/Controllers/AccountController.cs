using ETicaret.WebUI.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ETicaret.WebUI.Models;

namespace ETicaret.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]// Bu controllerdeki tüm post işlemlerinde validate token işlemleri yerine getirilir
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;// "UserManager" Microsoft Identity içerisinde olan bir yapı. UserManager'a kullanacağu "User(Identity klasöründeki)" bilgisini veririz. "_userManager" üzerinde kullanıcı oluşturma, login, parola sıfırlama gibi işlemlerimizi, metotlarımızı barındırıyor
        private SignInManager<User> _signInManager;// "SignInManager" Microsoft Identity içerisinde olan bir yapı. Bu da cookie olaylarını yönetecek

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login(string ReturnUrl = null)
        {
            //Url kısmında yazan RetunUrl kısmı ve sonraki url kısmını tanımlamış olduğumu "ReturnUrl" ye atarız ve onu Login.cshtml sayfasına göndeririz ki post metodun da ki yönlendirme işlemini yapalım.
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // var user = await _userManager.FindByNameAsync(model.UserName);// Veritabanında name(isime) göre bir arama yaptırırız eğer her hangi bir kullanıcı varsa "user" değişkenine atar.
            var user = await _userManager.FindByEmailAsync(model.Email);// Gönderilen mail bilgisi ile böyle biri var mı diye kontrol yapılacak
            if (user == null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı ile daha önce hesap oluşturulmamış");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);// Kayıtlı kullanıcı varsa kullanıcının tarayıcısına cooki bırakmak için bu kodları yazarız. İlk parametre bizden username bekler(model.user) yada user da yazabiliriz, ikinci parametre bizden password bekler, üçüncü parametre isPersistent özelliği ise oluşturulacak olan cookie'nin tarayıcı kapandığı anda silinip silinmeyeceğini belirler. Kullanıcının tarayıcısı kapandığı anda yada cookie'nin süresü doluyorsa bu parametreyi false yaparsak bu bilgi buradan silinir, true yapınca program.cs'de tanımladığımız süreyi saymaya başlar. Dördüncü parametre lockoutOnFailure ise kullanıcı program.cs tanımladığımız adet kadar başarısız bir giriş yaparsa hesabın kilitlenip kilitlenmeyeceğini burada belirtiriz, bu özelliğin açılıp açılmamasını burada belirtebiliriz. False yaparak hesap kilitleme işlemini burada kapatırız.

            if (result.Succeeded)
            {
                // Eğer kullanıcı login olmadan yetkisinin olmadığı yerlere giriş yapmak istediği zaman bizi login sayfasına yönlendirir. Yönlendirdiği zaman url kısmında "/account/login?ReturnUrl=%2Fadmin%2Fproducts" gibi query string den sonra gitmek istediğimiz sayfa url'si yazar. Login işlemi gerçekleştirildiği zaman istenilen sayfaya gidilmesi gerek. Eğer direkt login işlemi yapıldığı zaman anasayfaya yönlendirilebilir. Aşağıdaki kodda iki soru işareti ile null kontrolü yaparız ve ReturnUrl'nin null olup olmadığının kontrolünü yaparız. Login'nin get metodunda bunu Login.cshtml sayfasından (input type="hidden" name="ReturnUrl") olarak alırız. null ise anasayfaya, null değil ise istenilen sayfaya yönlendirilir
                return Redirect(model.ReturnUrl ?? "~/");
            }
            ModelState.AddModelError("", "Girilen kullanıcı adı veya parola yanlış");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();// "SignOutAsync" metodu cookie'yi tarayıcıdan siler ve cookie olmadığı için tekrardan login olmamız gerekir.
            //TempData.Put("message", new AlertMessage()
            //{
            //    Title = "Oturum Kapatıldı.",
            //    Message = "Hesabınız güvenli bir şekilde kapatıldı.",
            //    AlertType = "warning"
            //});
            return Redirect("~/");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]// get metodu ile gönderilen token bilgisi post metoduna gelmiyorsa işlemi gerçekleştirme. Böylelikle "CSRF" ataklarının önüne geçmiş oluruz
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)// ModelState' den gelen bilgi valid değilse 
            {
                // "RegisterModel" model içerisinde oluşturduğumuz property ve tanımladığımız dataannotations kuralları sağlanmıyorsa sayfada gösterilecek validasyonları ve girilen bilgileri "return View(model)" ekrana getirir. Eğer tüm değerler sağlanıyorsa if bloğu dışında ki kodları yapmaya başlar.
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
                // Sayfa üzerinden Password'ü userManager aracılığı ile kullanıcaz. O yüzden Password'ü "User" içerisine almıyoruz. Verilen Password bilgisi hashlenecek yani şifrelenerek sayfaya aktarılacak.
            };

            var result = await _userManager.CreateAsync(user, model.Password);// "CreateAsync" metodu bizden bir user bilgisi ve password bilgisi ister. Password'ü model üzerinden verdik.
            if (result.Succeeded)// Eğer bu işlem sonucunda bir kullanıcı oluşturulmuşsa aşağıdaki kodları yapar ve bunun sonucunda kullanıcıyı return ile login sayfasına yönlendiririz.
            {
                // generate token
                // email
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Bilinmeyen hata oldu lütfen tekrar deneyiniz.");// Uyguladığımız validasyonlar dışında herhangi bir hata çıkarsa diye yazdık. Bu hataları göstermek için "Register.cshtml" sayfasında "asp-validation-summary="All"" diyerek gösteririz. "All" yerine "ModelOnly" dersek sadece validasyon hatalarını gösteririz. "ModelState.AddModelError" metodunda ki ilk parametre(key bilgisi) herhangi bir property ile ilgili bir hata istersek yazarız. Hangi property(nam, password vb) yazarsak onun altında ikinci parametrede yazılan hata mesajını gösterir. İlk parametre boş bırakılırsa hiç bir parametre ile ilgilendirilmez ve oluşacak hatalar sayfanın üstünde gözükür.
            return View(model);
        }
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
