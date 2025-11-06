using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpoTrack.Data;
using ExpoTrack.Models;

namespace ExpoTrack.Controllers
{
    public class KullanicilarController : Controller
    {
        private readonly ExpoTrackContext _context;

        public KullanicilarController(ExpoTrackContext context)
        {
            _context = context;
        }

        // Kullanýcý listesini getirir
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kullanicilar.ToListAsync());
        }

        // Belirli bir kullanýcýnýn detaylarýný getirir
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(m => m.Id == id);
            if (kullanici == null) return NotFound();

            return View(kullanici);
        }

        // Yeni kullanýcý oluþturma formu
        public IActionResult Create()
        {
            return View();
        }

        // Yeni kullanýcý kaydýný veritabanýna ekler
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ad,Soyad,Email,Sifre,Rol,OlusturulmaTarihi")] Kullanicilar kullanici)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kullanici);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kullanici);
        }

        // Var olan kullanýcýyý düzenleme formu
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici == null) return NotFound();

            return View(kullanici);
        }

        // Kullanýcý bilgilerini günceller
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Soyad,Email,Sifre,Rol,OlusturulmaTarihi")] Kullanicilar kullanici)
        {
            if (id != kullanici.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kullanici);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KullanicilarExists(kullanici.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kullanici);
        }

        // Kullanýcý silme onay ekraný
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(m => m.Id == id);
            if (kullanici == null) return NotFound();

            return View(kullanici);
        }

        // Silme iþlemini gerçekleþtirir
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici != null)
            {
                _context.Kullanicilar.Remove(kullanici);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Verilen ID'ye sahip bir kullanýcý olup olmadýðýný kontrol eder
        private bool KullanicilarExists(int id)
        {
            return _context.Kullanicilar.Any(e => e.Id == id);
        }
    }
}
