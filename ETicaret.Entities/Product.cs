namespace ETicaret.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public bool IsHome { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }
    }
}
