using ETicaret.WebUI.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ETicaret.WebUI.Areas.Admin.Models
{
    public class RoleModel
    {
        [Required]
        public string Name { get; set; }
    }
    public class RoleDetails
    {
        public IdentityRole Role { get; set; } // Listeleme yapacağımız rolü seçmek için
        public IEnumerable<User> Members { get; set; }// Seçilen role ait kullanıcıları listelemek için tanımlandı
        public IEnumerable<User> NonMembers { get; set; }// Seçilen role ait olmayan kullanıcıları listelemek için tanımlandı
    }

    public class RoleEditModel
    {
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string[]? IdsToAdd { get; set; }// Seçilen role eklenebilecek kullanıcıların ıd'sini içeren dizi tanımladık
        public string[]? IdsToDelete { get; set; }// Seçilen rolden silinebilecek kullanıcıların ıd'sini içeren dizi tanımladık
    }
}
