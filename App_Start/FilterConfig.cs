using System.Web;
using System.Web.Mvc;

namespace Asp_Web_Lib
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
