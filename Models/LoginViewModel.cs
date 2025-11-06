using System.ComponentModel.DataAnnotations;

namespace ExpoTrack.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }
    }
}
