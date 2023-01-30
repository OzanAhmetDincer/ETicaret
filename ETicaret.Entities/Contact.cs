using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Entities
{
    internal class Contact : IEntity
    {
        public int Id { get; set; }
        
        [Display(Name ="Adınız"), Required, StringLength(50)]
        public string Name { get; set; }
        
        [Display(Name = "Soyadınız")]
        public string SurName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }

        [EmailAddress, StringLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Display(Name = "Telefon"), StringLength(15)]
        public string PhoneNumber { get; set; }
        public string HomePage { get; set; }
        
        [Display(Name = "Mesajınız"), StringLength(500)]
        public string Message { get; set; }
        
        [Display(Name = "Mesaj Tarihi"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; }



    }
}
