using System.ComponentModel.DataAnnotations;

namespace ETicaret.Entities
{
    public class News : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Başlık"), Required, StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "İçerik")]
        public string Content { get; set; }

        [Display(Name = "Resim"), StringLength(150)]
        public string Image { get; set; }

        [Display(Name = "Ekleme Tarihi"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; }
    }
}
