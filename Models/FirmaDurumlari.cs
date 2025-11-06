using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpoTrack.Models
{
    public class FirmaDurumlari
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Firmalar")]
        public int FirmalId { get; set; }

        [StringLength(50)]
        public string? Durum { get; set; }

        public string? Notlar { get; set; }

        public Firmalar? Firma { get; set; }
    }
}
