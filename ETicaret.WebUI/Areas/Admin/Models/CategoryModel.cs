using ETicaret.Entities;
using System.ComponentModel.DataAnnotations;
namespace ETicaret.WebUI.Areas.Admin.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), Display(Name = "Kategori Adı"), StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Kategori Açıklaması"), DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Kategori Resmi"), StringLength(150)]
        public string? Image { get; set; }

        [Display(Name = "Ekleme Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; } = DateTime.Now;// eğer bu alan boş geçilirse eklenme zamanını sistemden otomatik al

        [Display(Name = "Durum")]
        public bool IsActive { get; set; }

        [Display(Name = "Üst Menüde Göster")]
        public bool IsTopMenu { get; set; }

        [Display(Name = "Üst Kategori")]
        public int ParentId { get; set; }

        [Display(Name = "Kategori Sıra No")]
        public int OrderNo { get; set; }
        public int ProductId { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
