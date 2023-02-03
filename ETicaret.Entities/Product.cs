using System.ComponentModel.DataAnnotations;

namespace ETicaret.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} bu alan boş geçilemez!"), Display(Name = "Ürün Adı"), StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "Ürün Açıklaması")]
        public string? Description { get; set; }

        [Display(Name = "Ürün Resmi"), StringLength(150)]
        public string? Image { get; set; }

        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; }
        [Display(Name = "Güncelleme Tarihi"), ScaffoldColumn(false)]
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
        [Display(Name = "Durum")]
        public bool IsActive { get; set; }
        [Display(Name = "Anasayfa")]
        public bool IsHome { get; set; }
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
        [Display(Name = "Stok")]
        public int Stock { get; set; }
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }
        [Display(Name = "Kategori")]
        public virtual Category? Category { get; set; }// İki tablo arasında bağlantı kurmamızı sağlayacak. CategoryId üzerinden her bir post'un bir tane category'si olmuş olacak

        [Display(Name = "Marka")]
        public int BrandId { get; set; }
        [Display(Name = "Marka")]
        public Brand? Brand { get; set; }
    }
}
