using System;
using System.Web;
using System.Web.Mvc;
using Asp_Web_Lib.Helpers;

namespace Asp_Web_Lib.Controllers
{
    public class LanguageController : Controller
    {
        public ActionResult SetLanguage(string culture, string returnUrl)
        {
            // Walidacja kultury
            culture = CultureHelper.GetImplementedCulture(culture);

            // Zapisanie kultury w ciasteczku
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // Aktualizacja
            else
            {
                cookie = new HttpCookie("_culture")
                {
                    Value = culture,
                    Expires = DateTime.Now.AddYears(1)
                };
            }
            Response.Cookies.Add(cookie);

            return Redirect(returnUrl);
        }
    }
}