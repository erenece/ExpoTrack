using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ExpoTrack.Models
{
    public class Fuarlar
    {
        [Key]
        public int FuarId { get; set; }

        [StringLength(100)]
        [Display(Name = "Fuar Adı")]
        public string? Adi { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        public DateTime? BaslangicTarihi { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        public DateTime? BitisTarihi { get; set; }

        [StringLength(50)]

        [Display(Name = "Şehir")]
        public string? Sehir { get; set; }


        [StringLength(50)]
        [Display(Name = "Yer")]
        public string? Yer { get; set; }

        // İlişkiler
        public ICollection<FirmaFuarIliskileri>? FirmaFuarIliskileri { get; set; }
    }
}
