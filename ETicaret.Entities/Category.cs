using System.ComponentModel.DataAnnotations;

namespace ETicaret.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public bool IsTopMenu { get; set; }
        public int ParentId { get; set; }
        public int OrderNo { get; set; }
        public int ProductId { get; set; }
        public virtual ICollection<Product>? Products { get; set; }


    }
}
