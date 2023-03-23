using ETicaret.WebUI.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ETicaret.WebUI.Models;
using ETicaret.WebUI.EmailServices;
using ETicaret.WebUI.Extensions;
using ETicaret.Service.Abstract;

namespace ETicaret.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]// Bu controllerdeki tüm post işlemlerinde validate token işlemleri yerine getirilir
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;// "UserManager" Microsoft Identity içerisinde olan bir yapı. UserManager'a kullanacağu "User(Identity klasöründeki)" bilgisini veririz. "_userManager" üzerinde kullanıcı oluşturma, login, parola sıfırlama gibi işlemlerimizi, metotlarımızı barındırıyor
        private SignInManager<User> _signInManager;// "SignInManager" Microsoft Identity içerisinde olan bir yapı. Bu da cookie olaylarını yönetecek
        private IEmailSender _emailSender;
        private ICartService _cartService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICartService cartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cartService = cartService;
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

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                // Eğer kullanıcının EmailConfirm alanı false ise yani onaylanmamışsa kullanıcıyı hemen login yapmamak için kontrol ederiz. Kullanıcının tarayıcısına cookie göndermeden önce bunu burada kontrol ederiz.
                ModelState.AddModelError("", "Lütfen email hesabınıza gelen link ile üyeliğinizi onaylayınız.");
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
            TempData.Put("message", new AlertMessage()
            {
                Title = "Oturum Kapatıldı.",
                Message = "Hesabınız güvenli bir şekilde kapatıldı.",
                AlertType = "warning"
            });
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
                // generate token (kullanıcı hesabı başarılı bir şekilde oluştuduktan sonra benzersiz bir sayı üretip kullanıcıya mail ile göndericez. Biz burada bir url oluşturacaz ve oluşturduğumuz bu url ile hesap onaylama işlemi yapıcaz)

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);// "GenerateEmailConfirmationTokenAsync" metodu aracılığıyla buna bir user bilgisi veriyoruz ve verdiğimiz user bilgisi ile bize bir token oluşturuyor, oluşturulan token bilgisi veri tabanına kayıt ediliyor ve daha sonra biz bu token bilgisi ile onaylama işlemi yapıcaz. Bu onaylamada ilgili token ve url'nin eşleşmesi gerekir.

                //Aşağıdaki kod ile bir url oluştururuz. Accont controller altındaki ConfirmEmail metoduna bir parametre göndericez (/Account/ConfirmEmail/userId+token) gibi
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });

                // email
                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı onaylayınız.", $"Lütfen email hesabınızı onaylamak için linke <a href='https://localhost:7278{url}'>tıklayınız.</a>");
                // SendEmailAsync metodu içerisine yazılan ilk parametre hangi email adresine gönderilecek, ikinci parametre mailin başlığı, üçüncü parametre ise bir html mesaj
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Bilinmeyen hata oldu lütfen tekrar deneyiniz.");// Uyguladığımız validasyonlar dışında herhangi bir hata çıkarsa diye yazdık. Bu hataları göstermek için "Register.cshtml" sayfasında "asp-validation-summary="All"" diyerek gösteririz. "All" yerine "ModelOnly" dersek sadece validasyon hatalarını gösteririz. "ModelState.AddModelError" metodunda ki ilk parametre(key bilgisi) herhangi bir property ile ilgili bir hata istersek yazarız. Hangi property(nam, password vb) yazarsak onun altında ikinci parametrede yazılan hata mesajını gösterir. İlk parametre boş bırakılırsa hiç bir parametre ile ilgilendirilmez ve oluşacak hatalar sayfanın üstünde gözükür.
            return View(model);
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)// Dışarıdan iki parametre alıcaz. Biri userId yani hangi kullanıcının hesabı ise bize gelecek. İkinci parametre ise token bilgisi gelecek. Benzersiz bir sayı üreticez ve bu sayıyı kullanıcıya mail olarak göndericez. Kullanıcı maile tıkladığı zaman hesabı onaylanmış olacak.
        {
            if (userId == null || token == null)
            {
                //userId veya token null geldiyse hata mesajı verdiririz.
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Geçersiz token.",
                    Message = "Geçersiz Token",
                    AlertType = "danger"
                });
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);// userId ile eşleşen bir kullanıcı olup olmadığına bakarız.
            if (user != null)
            {
                // Eğer kullanıcı null'a eşit değilse hesabı onaylarız. Verdiğim user ve token bilgisi eşleşirse kullanıcının EmailConfirm alanını true yapar.
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    // Eğer onaylanma başarılı ise aşağıdaki mesajları göndeririz.
                    // cart objesini oluştur.
                    _cartService.InitializeCart(user.Id);

                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Hesabınız onaylandı.",
                        Message = "Hesabınız onaylandı.",
                        AlertType = "success"
                    });
                    return View();
                }
            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "Hesabınız onaylanmadı.",
                Message = "Hesabınız onaylanmadı.",
                AlertType = "warning"
            });
            return View();
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                // Eğer gelen mail bilgisi boşsa kullanıcıyı sayfaya tekrar göndeririz
                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);// İlk başta veritabanında mail adresi ile eşleşen böyle bir user var mı ona bakarız.

            if (user == null)
            {
                // Kullanıcı yoksa tekrardan sayfaya yönlendiririz. 
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);// Kullanıcı(user) varsa o kullanıcıya ait bir tane reset password vericez ve bu user ile ilişkilendirilecek. Benzersiz bir sayı oluşturulup bunun üzerinden reset işlemi yapılacak

            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = code
            });

            // email
            await _emailSender.SendEmailAsync(Email, "Reset Password", $"Parolanızı yenilemek için linke <a href='https://localhost:7278{url}'>tıklayınız.</a>");

            return View();
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            // Reset işlemi yapmamış için ResetPassword get metodu ile ResetPassword.cshtml sayfasında kullanıcının userId ve token bilgilerini almalıyızki sıfırlama işlemini yapalım. Bunuda ResetPassword.cshtml sayfasında "<input type="hidden" asp-for="Token">" şeklinde yaparız.
            if (userId == null || token == null)
            {
                return RedirectToAction("Home", "Index");
            }

            var model = new ResetPasswordModel { Token = token };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);// Girilen mail adresine ait user olup olmadığını kontrol ederiz ve "var user" değişkeninin içerisine atarız
            if (user == null)
            {
                return RedirectToAction("Home", "Index");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }


        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
