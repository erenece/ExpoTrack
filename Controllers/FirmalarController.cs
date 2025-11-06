using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpoTrack.Data;
using ExpoTrack.Models;
using OfficeOpenXml;

namespace ExpoTrack.Controllers
{
    // Bu controller firmalarýn yönetildiði, Excel'den içeri aktarma yapýldýðý ve fuar iliþkilerinin kurulduðu yerdir
    public class FirmalarController : BaseController
    {
        private readonly ExpoTrackContext _context;

        public FirmalarController(ExpoTrackContext context)
        {
            _context = context;
        }

        // GET: Excel dosyasý yükleme sayfasý
        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        // POST: Excel'den firmalarý ve fuar iliþkilerini içeri aktarma
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Lütfen bir Excel dosyasý seçin.");
                return View();
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++) // Ýlk satýr baþlýk
            {
                string fuarListesiText = worksheet.Cells[row, 1].Text.Trim();
                string[] fuarAdlari = fuarListesiText.Split(',', StringSplitOptions.RemoveEmptyEntries);

                var firma = new Firmalar
                {
                    FirmaAdi = worksheet.Cells[row, 2].Text,
                    Telefon = worksheet.Cells[row, 3].Text,
                    Email = worksheet.Cells[row, 4].Text,
                    WebSitesi = worksheet.Cells[row, 5].Text,
                    Yetkili = worksheet.Cells[row, 6].Text,
                    IsDurumu = worksheet.Cells[row, 7].Text,
                    KatildigiFuarlar = fuarListesiText
                };

                _context.Firmalar.Add(firma);
                await _context.SaveChangesAsync();

                foreach (var fuarAdi in fuarAdlari)
                {
                    var temizAdi = fuarAdi.Trim();
                    var fuar = await _context.Fuarlar.FirstOrDefaultAsync(f => f.Adi == temizAdi);

                    if (fuar == null)
                    {
                        fuar = new Fuarlar { Adi = temizAdi };
                        _context.Fuarlar.Add(fuar);
                        await _context.SaveChangesAsync();
                    }

                    _context.FirmaFuarIliskileri.Add(new FirmaFuarIliskileri
                    {
                        FirmaId = firma.Id,
                        FuarId = fuar.FuarId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Firma listesini getir
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var firmalar = await _context.Firmalar
                .Include(f => f.FuarIliskileri)
                    .ThenInclude(i => i.Fuar)
                .ToListAsync();

            return View(firmalar);
        }

        // Firma detaylarýný getir
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var firmalar = await _context.Firmalar.FirstOrDefaultAsync(m => m.Id == id);
            if (firmalar == null) return NotFound();

            return View(firmalar);
        }

        // Firma oluþturma formu
        [HttpGet]
        public IActionResult Create()
        {
            var tumFuarlar = _context.Fuarlar
                .Select(f => new SelectListItem
                {
                    Value = f.FuarId.ToString(),
                    Text = f.Adi
                }).ToList();

            var model = new FirmaEditViewModel
            {
                TumFuarlar = tumFuarlar
            };

            return View(model);
        }

        // Firma oluþtur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FirmaEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var firma = new Firmalar
                {
                    FirmaAdi = model.FirmaAdi,
                    Telefon = model.Telefon,
                    Email = model.Email,
                    WebSitesi = model.WebSitesi,
                    Yetkili = model.Yetkili,
                    IsDurumu = model.IsDurumu
                };

                _context.Firmalar.Add(firma);
                await _context.SaveChangesAsync();

                if (model.SecilenFuarlar != null)
                {
                    foreach (var fuarId in model.SecilenFuarlar)
                    {
                        _context.FirmaFuarIliskileri.Add(new FirmaFuarIliskileri
                        {
                            FirmaId = firma.Id,
                            FuarId = fuarId
                        });
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            model.TumFuarlar = _context.Fuarlar
                .Select(f => new SelectListItem
                {
                    Value = f.FuarId.ToString(),
                    Text = f.Adi
                }).ToList();

            return View(model);
        }

        // Firma düzenleme formu
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var firma = await _context.Firmalar
                .Include(f => f.FuarIliskileri)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (firma == null) return NotFound();

            var tumFuarlar = _context.Fuarlar
                .Select(f => new SelectListItem
                {
                    Value = f.FuarId.ToString(),
                    Text = f.Adi
                }).ToList();

            var model = new FirmaEditViewModel
            {
                Id = firma.Id,
                FirmaAdi = firma.FirmaAdi,
                Telefon = firma.Telefon,
                Email = firma.Email,
                WebSitesi = firma.WebSitesi,
                Yetkili = firma.Yetkili,
                IsDurumu = firma.IsDurumu,
                TumFuarlar = tumFuarlar,
                SecilenFuarlar = firma.FuarIliskileri.Select(x => x.FuarId).ToList()
            };

            return View(model);
        }

        // Firma düzenle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FirmaEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var firma = await _context.Firmalar
                    .Include(f => f.FuarIliskileri)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (firma == null) return NotFound();

                firma.FirmaAdi = model.FirmaAdi;
                firma.Telefon = model.Telefon;
                firma.Email = model.Email;
                firma.WebSitesi = model.WebSitesi;
                firma.Yetkili = model.Yetkili;
                firma.IsDurumu = model.IsDurumu;

                // Eski fuar iliþkilerini sil
                var eskiIliskiler = _context.FirmaFuarIliskileri.Where(x => x.FirmaId == id);
                _context.FirmaFuarIliskileri.RemoveRange(eskiIliskiler);

                // Yeni iliþkileri ekle
                if (model.SecilenFuarlar != null)
                {
                    foreach (var fuarId in model.SecilenFuarlar)
                    {
                        _context.FirmaFuarIliskileri.Add(new FirmaFuarIliskileri
                        {
                            FirmaId = id,
                            FuarId = fuarId
                        });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.TumFuarlar = _context.Fuarlar
                .Select(f => new SelectListItem
                {
                    Value = f.FuarId.ToString(),
                    Text = f.Adi
                }).ToList();

            return View(model);
        }

        // Firma silme sayfasý
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var firmalar = await _context.Firmalar.FirstOrDefaultAsync(m => m.Id == id);
            if (firmalar == null) return NotFound();

            return View(firmalar);
        }

        // Silme iþlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var firmalar = await _context.Firmalar.FindAsync(id);
            if (firmalar != null)
            {
                _context.Firmalar.Remove(firmalar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Ýþ Durumu: Mail Gönder olan firmalar
        [HttpGet]
        public async Task<IActionResult> MailBekleyen()
        {
            var firmalar = await _context.Firmalar
                .Include(f => f.FuarIliskileri).ThenInclude(fi => fi.Fuar)
                .Where(f => f.IsDurumu == "Mail Gönder")
                .ToListAsync();

            return View("Index", firmalar);
        }

        // Ýþ Durumu: Teklif Gönder olan firmalar
        [HttpGet]
        public async Task<IActionResult> TeklifBekleyen()
        {
            var firmalar = await _context.Firmalar
                .Include(f => f.FuarIliskileri).ThenInclude(fi => fi.Fuar)
                .Where(f => f.IsDurumu == "Teklif Gönder")
                .ToListAsync();

            return View("Index", firmalar);
        }

        // Ýþ Durumu: Sözleþme Gönder olan firmalar
        [HttpGet]
        public async Task<IActionResult> Sozlesme()
        {
            var firmalar = await _context.Firmalar
                .Include(f => f.FuarIliskileri).ThenInclude(fi => fi.Fuar)
                .Where(f => f.IsDurumu == "Sözleþme Gönder")
                .ToListAsync();

            return View("Index", firmalar);
        }

        // Firma var mý kontrolü
        private bool FirmalarExists(int id)
        {
            return _context.Firmalar.Any(e => e.Id == id);
        }
    }
}
