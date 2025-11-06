using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpoTrack.Models
{
    public class Kullanicilar
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]

        [Display(Name = "Ad")]
        public string? Ad { get; set; }

        [StringLength(50)]

        [Display(Name = "Soyad")]
        public string? Soyad { get; set; }

        [StringLength(50)]
        [Display(Name = "E Mail")]
        public string? Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Şifre")]
        public string? Sifre { get; set; }

        [StringLength(50)]
        [Display(Name = "Rol")]
        public string? Rol { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime? OlusturulmaTarihi { get; set; }
    }
}
