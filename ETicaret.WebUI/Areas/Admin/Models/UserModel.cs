using System.ComponentModel.DataAnnotations;
namespace ETicaret.WebUI.Areas.Admin.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Ad"), StringLength(50), Required(ErrorMessage = "{0} alanı boş geçilemez!")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad"), StringLength(50), Required(ErrorMessage = "{0} alanı boş geçilemez!")]
        public string LastName { get; set; }

        [Display(Name = "Kullanıcı Adı"), StringLength(50), Required(ErrorMessage = "{0} alanı boş geçilemez!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), EmailAddress, StringLength(50)]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }// Kullanıcının seçmiş olduğu role bilgilerini sayfaya taşımak için oluşturduk. Kullanıcının o andaki seçmiş olduğu rol bilgileri
    }
}
