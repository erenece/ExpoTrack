using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpoTrack.Models
{
    public class FirmaFuarIliskileri
    {
        [Key]

        public int Id { get; set; }

        [ForeignKey("Fuarlar")]
        public int FuarId { get; set; }

        [ForeignKey("Firmalar")]
        //public int FirmalId { get; set; }
        public int FirmaId { get; set; }

        public Fuarlar Fuar { get; set; }
        public Firmalar? Firma { get; set; }
    }
}
