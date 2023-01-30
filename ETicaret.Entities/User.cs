using System.ComponentModel.DataAnnotations;

namespace ETicaret.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Ad"), StringLength(50), Required(ErrorMessage ="{0} alanı boş geçilemez!") ]
        public string Name { get; set; }

        [Display(Name ="Soyad"), StringLength(50), Required(ErrorMessage ="{0} alanı boş geçilemez!")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), EmailAddress, StringLength(50)]
        public string Email { get; set; }

        [Display(Name ="Telefon"), StringLength(20)]
        public string? Phone { get; set; }

        [Display(Name ="Şifre"), StringLength(50), Required(ErrorMessage ="{0} alanı boş geçilemez")]
        public string Password { get; set; }

        [Display(Name ="Durum")]
        public bool IsActive { get; set; }

        [Display(Name = "Admin")]
        public bool IsAdmin { get; set; }

        [Display(Name ="Ekleme Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; }= DateTime.Now;

    }
}
