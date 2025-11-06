using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpoTrack.Models
{
    public class FirmaEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Firma Adı")]
        public string FirmaAdi { get; set; }

        [Display(Name = "Telefon")]
        public string Telefon { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Web Sitesi")]
        public string WebSitesi { get; set; }

        [Display(Name = "Yetkili")]
        public string Yetkili { get; set; }

        [Display(Name = "İş Durumu")]

        public string IsDurumu { get; set; }


        public List<int> SecilenFuarlar { get; set; } = new List<int>();
        public List<SelectListItem> TumFuarlar { get; set; } = new List<SelectListItem>();

        public string Sehir { get; set; } = "";
        public List<SelectListItem> Sehirler { get; set; } = new List<SelectListItem>();

    }
}
