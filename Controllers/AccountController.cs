using Microsoft.AspNetCore.Mvc;
using ExpoTrack.Models;
using ExpoTrack.Data;
using System.Linq;

namespace ExpoTrack.Controllers
{
    // Kullanıcı girişi ve çıkışı işlemlerini yöneten controller
    public class AccountController : Controller
    {
        private readonly ExpoTrackContext _context;

        // Veritabanı bağlantısını constructor üzerinden alıyoruz
        public AccountController(ExpoTrackContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        // Kullanıcıya giriş formunu gösterir
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        // Kullanıcı giriş bilgilerini işler
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // Model geçerliyse (tüm gerekli alanlar doluysa)
            if (ModelState.IsValid)
            {
                // Veritabanında e-posta ve şifreyle eşleşen kullanıcıyı bul
                var kullanici = _context.Kullanicilar
                    .FirstOrDefault(x => x.Email == model.Email && x.Sifre == model.Sifre);

                if (kullanici != null)
                {
                    // Oturum bilgilerini sakla (giriş yapan kullanıcının email ve rolü)
                    HttpContext.Session.SetString("Email", kullanici.Email ?? "");
                    HttpContext.Session.SetString("Rol", kullanici.Rol ?? "");

                    // Giriş başarılı → Ana sayfaya yönlendir
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Giriş başarısızsa uyarı mesajı göster
                    ModelState.AddModelError("", "Geçersiz email veya şifre");
                }
            }

            // Eğer model geçerli değilse ya da kullanıcı bulunamazsa aynı sayfayı tekrar göster
            return View(model);
        }

        // GET: /Account/Logout
        // Kullanıcı çıkış yaparsa session'ı temizle ve login sayfasına yönlendir
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Tüm oturum bilgilerini temizler
            return RedirectToAction("Login", "Account");
        }
    }
}
