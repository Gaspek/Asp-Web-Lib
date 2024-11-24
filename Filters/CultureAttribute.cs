using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Asp_Web_Lib.Helpers;

namespace Asp_Web_Lib.Filters
{
    public class CultureAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string cultureName = null;

            // Próba pobrania kultury z ciasteczka
            HttpCookie cultureCookie = filterContext.HttpContext.Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = filterContext.RouteData.Values["culture"] as string;

            // Walidacja nazwy kultury
            cultureName = CultureHelper.GetImplementedCulture(cultureName);

            // Ustawienie kultury dla wątku
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);

            base.OnActionExecuting(filterContext);
        }
    }
}