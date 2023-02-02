using System.ComponentModel.DataAnnotations;

namespace ETicaret.Entities
{
    public class Product : IEntity
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
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }// İki tablo arasında bağlantı kurmamızı sağlayacak. CategoryId üzerinden her bir post'un bir tane category'si olmuş olacak
    }
}
