namespace ExpoTrack.Models
{
    // Hata sayfasında kullanılmak üzere basit bir model
    public class ErrorViewModel
    {
        // Hata isteğine ait özel tanımlayıcı (genellikle loglarda kullanılır)
        public string? RequestId { get; set; }

        // RequestId boş değilse kullanıcıya gösterilsin mi? (true/false)
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
