using ExpoTrack.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpoTrack.Controllers
{
    // Ana sayfa ve sistemsel sayfalarý yöneten controller
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        // Loglama iþlemleri için constructor üzerinden ILogger alýnýyor
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Giriþ (ana) sayfa
        public IActionResult Index()
        {
            return View();
        }

        // Gizlilik politikasý sayfasý (þu an sabit içerikli View döner)
        public IActionResult Privacy()
        {
            return View();
        }

        // Hata sayfasý - sistemsel hatalarý yakalayýp kullanýcýya gösterir
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Hata durumunda gösterilecek ViewModel oluþturuluyor
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
