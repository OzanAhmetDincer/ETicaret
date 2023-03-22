using System.ComponentModel.DataAnnotations;

namespace ETicaret.WebUI.Models
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]// Yukarıda tanımladığımız "Password" değişkeni ile karşılaştırma yapıp, aynı olup olmadığını kontrol edecek.
        public string RePassword { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
