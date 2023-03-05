using System.ComponentModel.DataAnnotations;
namespace ETicaret.WebUI.Areas.Admin.Models
{
    public class ContactModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), Display(Name = "Ad"), StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), StringLength(50), Display(Name = "Soyad")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), StringLength(50), EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Telefon"), StringLength(20), DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), Display(Name = "Mesaj"), StringLength(500)]
        public string Message { get; set; }

        [Display(Name = "Mesaj Tarihi"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; }
    }
}
