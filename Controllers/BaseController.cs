using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// Tüm controller'lar için ortak oturum kontrolünü yapan sınıf
public class BaseController : Controller
{
    // Her action çalışmadan önce bu metod tetiklenir
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Session’dan email bilgisini al (oturum var mı diye kontrol ediyoruz)
        var email = context.HttpContext.Session.GetString("Email");

        // Eğer kullanıcı giriş yapmamışsa ve istek Account/Login'e değilse
        if (string.IsNullOrEmpty(email)
            && !(context.RouteData.Values["controller"]?.ToString() == "Account"
                && context.RouteData.Values["action"]?.ToString() == "Login"))
        {
            // Kullanıcıyı Login sayfasına yönlendir
            context.Result = new RedirectToActionResult("Login", "Account", null);
        }

        // Varsayılan davranışı devam ettir
        base.OnActionExecuting(context);
    }
}
