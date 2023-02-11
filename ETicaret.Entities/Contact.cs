using System.ComponentModel.DataAnnotations;

namespace ETicaret.Entities
{
    public class Contact : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }



    }
}
