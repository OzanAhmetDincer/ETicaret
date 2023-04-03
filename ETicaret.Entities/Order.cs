using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Entities
{
    public class Order : IEntity
    {
        public int Id { get; set; }// Siparişin Id numarası
        public string OrderNumber { get; set; } // Siparişler için benzersiz bir rakam oluşturabiliriz. İsteğe bağlı  bir alan. Siparişler için Id bilgisinide kullanabiliriz.
        public DateTime OrderDate { get; set; }// Siparişin verilme tarihi
        public string UserId { get; set; }// Siparişi kimin verdiğini alabilmemiz için. Biz bu UserId bilgisi aracılığı ile(yabancıl anahtar) Identity tablosuna geçiş yapabiliriz ve User tablosu üzerindeki bilgileri direkt kullanabiliriz ancak sipariş bir başkası adına da verilmiş olabilir. O yüzden aşağıda ki bilgileride almamız gerekir.    
        public string FirstName { get; set; }// Siparişi alacak kişinin ismi
        public string LastName { get; set; }// Siparişi alacak kişinin soyismi
        public string Address { get; set; }// Siparişi alacak kişinin adresi
        public string City { get; set; }// Siparişi alacak kişinin şehiri
        public string Email { get; set; }// Siparişi alacak kişinin emaili
        public string Phone { get; set; }// Siparişi alacak kişinin telefonu
        public string Note { get; set; }// Sipariş notu
        public string PaymentId { get; set; }
        public string ConversationId { get; set; }
        public List<OrderItem> OrderItems { get; set; }// Kullanıcının siparişte hangi ürünleri verdiğini görmek için
        public EnumPaymentType PaymentType { get; set; }
        public EnumOrderState OrderState { get; set; }


        public enum EnumPaymentType
        {
            CreditCard = 0,
            Eft = 1
        }

        public enum EnumOrderState// Siparişin durumu
        {
            waiting = 0,// onay bekliyor
            unpaid = 1,// Ödenmemiş 
            completed = 2// Tamamlandı
        }

    }
}
