using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Asp_Web_Lib
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Pobierz aktualn� �cie�k� URL
            var url = HttpContext.Current.Request.Url.LocalPath;

            // Sprawd�, czy �cie�ka zawiera parametr culture
            var segments = url.Trim('/').Split('/');
            if (segments.Length > 0 && IsCultureSegment(segments[0]))
            {
                // Je�li pierwszy segment to kultura, nic nie r�b
                return;
            }

            // Je�li brak kultury w URL, ustaw domy�ln�
            var culture = "pl"; // Domy�lna kultura
            var newUrl = $"/{culture}{url}";

            // Przekierowanie do nowego URL
            HttpContext.Current.Response.Redirect(newUrl, true);
        }

        private bool IsCultureSegment(string segment)
        {
            // Lista obs�ugiwanych kultur
            var supportedCultures = new[] { "en", "pl" };
            return supportedCultures.Contains(segment.ToLower());
        }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

    }
}
