using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ExpoTrack.Models
{
    public class FuarEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Fuar Adı")]
        public string Adi { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        public DateTime? BaslangicTarihi { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        public DateTime? BitisTarihi { get; set; }

        [Display(Name = "Şehir")]
        public string Sehir { get; set; } = "";

        [Display(Name = "Şehirler")]
        public List<SelectListItem> Sehirler { get; set; } = new();

        [Display(Name = "Yer")]
        public string? Yer { get; set; }
    }
}
