using Microsoft.EntityFrameworkCore;
using ExpoTrack.Models;

namespace ExpoTrack.Data
{
    // Veritabanı ile uygulama arasındaki bağlantıyı yöneten sınıf
    public class ExpoTrackContext : DbContext
    {
        public ExpoTrackContext(DbContextOptions<ExpoTrackContext> options)
            : base(options)
        {
        }

        // Firmalar tablosu ile ilişki
        public DbSet<Firmalar> Firmalar { get; set; }

        // Fuarlar tablosu ile ilişki
        public DbSet<Fuarlar> Fuarlar { get; set; }

        // Firma-Fuar ilişkilerini tutan ara tablo
        public DbSet<FirmaFuarIliskileri> FirmaFuarIliskileri { get; set; }

        // Kullanıcıları tutan tablo
        public DbSet<Kullanicilar> Kullanicilar { get; set; }
    }
}
