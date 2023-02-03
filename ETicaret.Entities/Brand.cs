using System.ComponentModel.DataAnnotations;

namespace ETicaret.Entities
{
    public class Brand : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), StringLength(50), Display(Name = "Marka Adı")]
        public string Name { get; set; }

        [Display(Name = "Marka Açıklaması")]
        public string Description { get; set; }

        [StringLength(150)]
        public string? Logo { get; set; } = "";// property e varsayılan değer atama. Yani biz burada logo yu boş bırakınca o tırnaklar içindeki değer girilir.

        [Display(Name = "Durum")]
        public bool IsActive { get; set; }

        [Display(Name = "Ekleme Tarihi"), ScaffoldColumn(false)]// ScaffoldColumn(false) demek view lar oluştururken bu alan ekranda oluşturulmasın demektir
        public DateTime? CreateDate { get; set; }
        public virtual ICollection<Product>? Products { get; set; }// Marka ile Ürünler arasında 1 e çok ilişki kurduk

    }
}
