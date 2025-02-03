using Asp_Web_Lib.Models;
using Asp_Web_Lib.Services;
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
        private static QueueObserver _queueObserver;
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Pobierz aktualn¹ œcie¿kê URL
            var url = HttpContext.Current.Request.Url.LocalPath;

            // SprawdŸ, czy œcie¿ka zawiera parametr culture
            var segments = url.Trim('/').Split('/');
            if (segments.Length > 0 && IsCultureSegment(segments[0]))
            {
                // Jeœli pierwszy segment to kultura, nic nie rób
                return;
            }

            // Jeœli brak kultury w URL, ustaw domyœln¹
            var culture = "pl"; // Domyœlna kultura
            var newUrl = $"/{culture}{url}";

            // Przekierowanie do nowego URL
            HttpContext.Current.Response.Redirect(newUrl, true);
        }

        private bool IsCultureSegment(string segment)
        {
            // Lista obs³ugiwanych kultur
            var supportedCultures = new[] { "en", "pl" };
            return supportedCultures.Contains(segment.ToLower());
        }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InitializeQueueObserver();
        }

        private void InitializeQueueObserver()
        {
            var dbContext = new ApplicationDbContext();
            var bookService = new BookService(dbContext);
            _queueObserver = new QueueObserver(bookService, dbContext);
        }


    }
}
