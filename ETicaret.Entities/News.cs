namespace ETicaret.Entities
{
    public class News : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
