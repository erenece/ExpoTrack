using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpoTrack.Data;
using ExpoTrack.Models;

namespace ExpoTrack.Controllers
{
    public class FuarlarController : BaseController
    {
        private readonly ExpoTrackContext _context;

        public FuarlarController(ExpoTrackContext context)
        {
            _context = context;
        }

        // Tüm fuarları listeler
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fuarlar.ToListAsync());
        }

        // Fuar detay sayfası
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var fuar = await _context.Fuarlar.FirstOrDefaultAsync(m => m.FuarId == id);
            if (fuar == null) return NotFound();

            return View(fuar);
        }

        // Belirli bir fuara katılan firmalar
        public async Task<IActionResult> Katilimcilar(int id)
        {
            var firmalar = await _context.FirmaFuarIliskileri
                .Include(f => f.Firma)
                .Where(x => x.FuarId == id && x.Firma != null)
                .Select(x => x.Firma)
                .ToListAsync();

            ViewBag.FuarAdi = await _context.Fuarlar
                .Where(f => f.FuarId == id)
                .Select(f => f.Adi)
                .FirstOrDefaultAsync();

            return View(firmalar);
        }

        // Yeni fuar oluşturma formu
        public IActionResult Create()
        {
            var model = new FuarEditViewModel
            {
                Sehirler = SehirListesiGetir()
            };

            return View(model);
        }

        // Yeni fuarı kaydet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FuarEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var fuar = new Fuarlar
                {
                    Adi = model.Adi,
                    BaslangicTarihi = model.BaslangicTarihi,
                    BitisTarihi = model.BitisTarihi,
                    Sehir = model.Sehir,
                    Yer = model.Yer
                };

                _context.Fuarlar.Add(fuar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.Sehirler = SehirListesiGetir();
            return View(model);
        }

        // Fuar düzenleme formu
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var fuar = await _context.Fuarlar.FindAsync(id);
            if (fuar == null) return NotFound();

            var model = new FuarEditViewModel
            {
                Id = fuar.FuarId,
                Adi = fuar.Adi,
                BaslangicTarihi = fuar.BaslangicTarihi,
                BitisTarihi = fuar.BitisTarihi,
                Sehir = fuar.Sehir ?? "",
                Yer = fuar.Yer,
                Sehirler = SehirListesiGetir()
            };

            return View(model);
        }

        // Fuar düzenlemesini kaydet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FuarEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var fuar = await _context.Fuarlar.FindAsync(id);
                if (fuar == null) return NotFound();

                fuar.Adi = model.Adi;
                fuar.BaslangicTarihi = model.BaslangicTarihi;
                fuar.BitisTarihi = model.BitisTarihi;
                fuar.Sehir = model.Sehir;
                fuar.Yer = model.Yer;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.Sehirler = SehirListesiGetir();
            return View(model);
        }

        // Silme onay sayfası
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var fuar = await _context.Fuarlar.FirstOrDefaultAsync(m => m.FuarId == id);
            if (fuar == null) return NotFound();

            return View(fuar);
        }

        // Silme işlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fuar = await _context.Fuarlar.FindAsync(id);
            if (fuar != null)
            {
                _context.Fuarlar.Remove(fuar);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Fuar varsa true döner
        private bool FuarlarExists(int id)
        {
            return _context.Fuarlar.Any(e => e.FuarId == id);
        }

        // Şehir dropdown'ı için liste
        private List<SelectListItem> SehirListesiGetir()
        {
            var sehirler = "adana adıyaman afyon ağrı amasya ankara antalya artvin aydın balıkesir bilecik bingöl bitlis bolu burdur bursa çanakkale çankırı çorum denizli diyarbakır edirne elazığ erzincan erzurum eskişehir gaziantep giresun gümüşhane hakkari hatay ısparta mersin istanbul izmir kars kastamonu kayseri kırklareli kırşehir kocaeli konya kütahya malatya manisa kahramanmaraş mardin muğla muş nevşehir niğde ordu rize sakarya samsun siirt sinop sivas tekirdağ tokat trabzon tunceli şanlıurfa uşak van yozgat zonguldak aksaray bayburt karaman bartın ardahan ığdır yalova karabük kilis osmaniye";

            return System.Globalization.CultureInfo
                .GetCultureInfo("tr-TR")
                .TextInfo
                .ToTitleCase(sehirler)
                .Split(' ')
                .Select(s => new SelectListItem { Value = s, Text = s })
                .ToList();
        }
    }
}
