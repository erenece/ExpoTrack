using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpoTrack.Data;
using ExpoTrack.Models;

namespace ExpoTrack.Controllers
{
    // Firma-Fuar eþleþtirmelerini yöneten controller
    public class FirmaFuarIliskileriController : Controller
    {
        private readonly ExpoTrackContext _context;

        public FirmaFuarIliskileriController(ExpoTrackContext context)
        {
            _context = context;
        }

        // Tüm iliþkileri listele
        public async Task<IActionResult> Index()
        {
            // Fuar bilgisi dahil edilerek tüm kayýtlar alýnýr
            var iliskiler = _context.FirmaFuarIliskileri
                .Include(f => f.Fuar)
                .Include(f => f.Firma);
            return View(await iliskiler.ToListAsync());
        }

        // Detay sayfasý
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var iliski = await _context.FirmaFuarIliskileri
                .Include(f => f.Fuar)
                .Include(f => f.Firma)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (iliski == null)
                return NotFound();

            return View(iliski);
        }

        // Yeni iliþki oluþturma formu
        public IActionResult Create()
        {
            // Fuar ve Firma seçim listeleri
            ViewData["FuarId"] = new SelectList(_context.Fuarlar, "FuarId", "Adi");
            ViewData["FirmaId"] = new SelectList(_context.Firmalar, "Id", "FirmaAdi");
            return View();
        }

        // Yeni iliþkiyi kaydet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FuarId,FirmaId")] FirmaFuarIliskileri iliski)
        {
            if (ModelState.IsValid)
            {
                _context.Add(iliski);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["FuarId"] = new SelectList(_context.Fuarlar, "FuarId", "Adi", iliski.FuarId);
            ViewData["FirmaId"] = new SelectList(_context.Firmalar, "Id", "FirmaAdi", iliski.FirmaId);
            return View(iliski);
        }

        // Ýliþki düzenleme formu
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var iliski = await _context.FirmaFuarIliskileri.FindAsync(id);
            if (iliski == null)
                return NotFound();

            ViewData["FuarId"] = new SelectList(_context.Fuarlar, "FuarId", "Adi", iliski.FuarId);
            ViewData["FirmaId"] = new SelectList(_context.Firmalar, "Id", "FirmaAdi", iliski.FirmaId);
            return View(iliski);
        }

        // Düzenlenen iliþkiyi kaydet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FuarId,FirmaId")] FirmaFuarIliskileri iliski)
        {
            if (id != iliski.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iliski);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FirmaFuarIliskileriExists(iliski.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["FuarId"] = new SelectList(_context.Fuarlar, "FuarId", "Adi", iliski.FuarId);
            ViewData["FirmaId"] = new SelectList(_context.Firmalar, "Id", "FirmaAdi", iliski.FirmaId);
            return View(iliski);
        }

        // Silme ekraný
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var iliski = await _context.FirmaFuarIliskileri
                .Include(f => f.Fuar)
                .Include(f => f.Firma)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (iliski == null)
                return NotFound();

            return View(iliski);
        }

        // Silme iþlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var iliski = await _context.FirmaFuarIliskileri.FindAsync(id);
            if (iliski != null)
                _context.FirmaFuarIliskileri.Remove(iliski);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Ýliþki var mý kontrolü
        private bool FirmaFuarIliskileriExists(int id)
        {
            return _context.FirmaFuarIliskileri.Any(e => e.Id == id);
        }
    }
}
