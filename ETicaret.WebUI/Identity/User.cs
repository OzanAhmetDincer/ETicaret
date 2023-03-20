using Microsoft.AspNetCore.Identity;

namespace ETicaret.WebUI.Identity
{
    // "Identity" klasörü içerisine üyelik işlemlerine ait özellikleri tanımlarız. Bu işlemleri yapabilmek için nuget olarak "Microsoft.AspNetCore.Identity.EntityFrameworkCore", "Microsoft.AspNetCore.Identity.UI" ekleriz.
    // "IdentityUser" class'ından türetilmiş "User" adında bir class tanımladık. "IdentityUser" class'ı içerisinde kullanıcıya ait isim, soyisim, telefon, şifre gibi bilgiler tanımlı bir şekilde gelir. Bu bilgiler "User" class'ına dahil ediliyor. Bunun dışında aşağıda olduğu gibi fazladan özellikte tanımlayabiliriz(cinsiyet vb.)
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
