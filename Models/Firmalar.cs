using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ExpoTrack.Models
{
    public class Firmalar
    {
        [Key]
        public int Id { get; set; }

        [Required]

        [Display(Name = "Firma Adı")]
        public string FirmaAdi { get; set; }

        [Display(Name = "Telefon")]
        public string? Telefon { get; set; }

        [Display(Name = "E Mail")]
        public string? Email { get; set; }

        [Display(Name = "Web Sitesi")]
        public string? WebSitesi { get; set; }

        [Display(Name = "Yetkili")]
        public string? Yetkili { get; set; }

        [Display(Name = "İş Durumu")]
        public string? IsDurumu { get; set; }

        [Display(Name = "Katılığı Fuarlar")]
        public string? KatildigiFuarlar { get; set; }









        public ICollection<FirmaFuarIliskileri>? FuarIliskileri { get; set; }


        
    
        
    }
}
