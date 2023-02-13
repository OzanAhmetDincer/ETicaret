using System.ComponentModel.DataAnnotations;

namespace ETicaret.WebUI.Areas.Admin.Models
{
    public class NewsModel
    {
        public int Id { get; set; }

        [Display(Name = "Başlık"), Required(ErrorMessage = "{0} alanı boş geçilemez!")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Kategori için 4-100 arasında değer giriniz.")]
        public string Name { get; set; }

        [Display(Name = "İçerik"), Required(ErrorMessage = "{0} alanı boş geçilemez!")]
        [StringLength(500, MinimumLength = 4, ErrorMessage = "Kategori için 4-500 arasında değer giriniz.")]
        public string Content { get; set; }

        [Display(Name = "Resim"), StringLength(150)]
        public string? Image { get; set; }

        [Display(Name = "Ekleme Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
    }
}
