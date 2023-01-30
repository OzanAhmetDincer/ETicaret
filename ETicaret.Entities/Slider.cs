using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Entities
{
    public class Slider : IEntity
    {
        public int Id { get; set; }

        [Display(Name ="Başlık"), StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "Açıklama"), DataType(DataType.MultilineText), StringLength(250)]
        public string Description { get; set; }
        public string Link { get; set; }

        [Display(Name ="Resim")]
        public string Image { get; set; }
    }
}
