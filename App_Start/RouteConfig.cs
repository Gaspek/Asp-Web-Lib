using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Asp_Web_Lib.Filters;

namespace Asp_Web_Lib
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Trasa z kulturą jako częścią URL
            routes.MapRoute(
                name: "LocalizedDefault",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new { culture = "en", controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            // Domyślna trasa dla zgodności
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

    }
}
