using System.ComponentModel.DataAnnotations;

namespace ETicaret.WebUI.Areas.Admin.Models
{
    public class SliderModel
    {
        public int Id { get; set; }

        [Display(Name = "Başlık"), StringLength(150)]
        public string? Title { get; set; }

        [Display(Name = "Açıklama"), DataType(DataType.MultilineText), StringLength(500)]
        public string? Description { get; set; }
        public string? Link { get; set; }

        [Display(Name = "Resim"), StringLength(150)]
        public string? Image { get; set; }
    }
}
