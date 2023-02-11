using ETicaret.Entities;
using System.ComponentModel.DataAnnotations;

namespace ETicaret.WebUI.Areas.Admin.Models
{
    public class NewsModel
    {
        public int Id { get; set; }

        [Display(Name = "Başlık"), Required(ErrorMessage = "{0} alanı boş geçilemez!"), StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "İçerik")]
        public string Content { get; set; }

        [Display(Name = "Resim"), StringLength(150)]
        public string? Image { get; set; }

        [Display(Name = "Ekleme Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; }
        public List<News> Haberler { get; set; }
    }
}
