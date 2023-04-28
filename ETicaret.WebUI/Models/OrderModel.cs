using System.ComponentModel.DataAnnotations;

namespace ETicaret.WebUI.Models
{
    public class OrderModel
    {
        // Siparişi veren kullanıcının kişisel ve cart bilgilerini tanımladık
        [Display(Name = "Ad")]
        public string FirstName { get; set; }
        [Display(Name = "Soyad")]
        public string LastName { get; set; }
        [Display(Name = "Adres")]
        public string Address { get; set; }
        [Display(Name = "Şehir")]
        public string City { get; set; }
        [Display(Name = "Telefon"), DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Display(Name = "Açıklama")]
        public string Note { get; set; }
        [Display(Name = "Email"),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Kart Adı")]
        public string CardName { get; set; }
        [Display(Name = "Kart Numarası"), DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }
        [Display(Name = "Ay")]
        public string ExpirationMonth { get; set; }
        [Display(Name = "Yıl")]
        public string ExpirationYear { get; set; }
        [Display(Name = "Cvc")]
        public string Cvc { get; set; }
        public CartModel CartModel { get; set; }// Özet bilgiyi göstermek için tanımladık
    }
}
