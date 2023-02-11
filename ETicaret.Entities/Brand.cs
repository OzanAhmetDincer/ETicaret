using System.ComponentModel.DataAnnotations;

namespace ETicaret.Entities
{
    public class Brand : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Logo { get; set; } = "";// property e varsayılan değer atama. Yani biz burada logo yu boş bırakınca o tırnaklar içindeki değer girilir.
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public virtual ICollection<Product>? Products { get; set; }

    }
}
