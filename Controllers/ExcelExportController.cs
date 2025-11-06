using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpoTrack.Data;
using System.IO;
using System.Linq;

namespace ExpoTrack.Controllers
{
    // Excel'e firma verilerini aktaran controller
    public class ExcelExportController : Controller
    {
        private readonly ExpoTrackContext _context;

        public ExcelExportController(ExpoTrackContext context)
        {
            _context = context;
        }

        // /ExcelExport/ExportFirmalarToExcel → Excel çıktısı döner
        public IActionResult ExportFirmalarToExcel()
        {
            // Firmaları ve ilişkili olduğu fuarları veritabanından çek
            var firmalar = _context.Firmalar
                .Include(f => f.FuarIliskileri)
                    .ThenInclude(i => i.Fuar)
                .ToList();

            // Yeni bir Excel çalışma kitabı (workbook) oluştur
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Firmalar");
                var currentRow = 1;

                // Başlık satırını yaz
                worksheet.Cell(currentRow, 1).Value = "Fuar Adı";
                worksheet.Cell(currentRow, 2).Value = "Firma Adı";
                worksheet.Cell(currentRow, 3).Value = "Telefon";
                worksheet.Cell(currentRow, 4).Value = "Email";
                worksheet.Cell(currentRow, 5).Value = "Web Sitesi";
                worksheet.Cell(currentRow, 6).Value = "Yetkili";
                worksheet.Cell(currentRow, 7).Value = "İş Durumu";

                // Her firmayı ve varsa katıldığı fuarları satır satır ekle
                foreach (var firma in firmalar)
                {
                    // Firma fuara katılmışsa, her fuar için ayrı satır yaz
                    if (firma.FuarIliskileri != null && firma.FuarIliskileri.Any())
                    {
                        foreach (var iliski in firma.FuarIliskileri)
                        {
                            var fuarAdi = iliski.Fuar?.Adi ?? "Fuarsız";
                            currentRow++;

                            worksheet.Cell(currentRow, 1).Value = fuarAdi;
                            worksheet.Cell(currentRow, 2).Value = firma.FirmaAdi;
                            worksheet.Cell(currentRow, 3).Value = firma.Telefon;
                            worksheet.Cell(currentRow, 4).Value = firma.Email;
                            worksheet.Cell(currentRow, 5).Value = firma.WebSitesi;
                            worksheet.Cell(currentRow, 6).Value = firma.Yetkili;
                            worksheet.Cell(currentRow, 7).Value = firma.IsDurumu;
                        }
                    }
                    else // Fuara katılmamışsa yine de ekle
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = "Fuarsız";
                        worksheet.Cell(currentRow, 2).Value = firma.FirmaAdi;
                        worksheet.Cell(currentRow, 3).Value = firma.Telefon;
                        worksheet.Cell(currentRow, 4).Value = firma.Email;
                        worksheet.Cell(currentRow, 5).Value = firma.WebSitesi;
                        worksheet.Cell(currentRow, 6).Value = firma.Yetkili;
                        worksheet.Cell(currentRow, 7).Value = firma.IsDurumu;
                    }
                }

                // Excel dosyasını hafızaya kaydet ve kullanıcıya indirttir
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    // Dosyayı .xlsx formatında tarayıcıya gönder
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Firmalar.xlsx");
                }
            }
        }
    }
}
