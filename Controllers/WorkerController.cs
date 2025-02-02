using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Asp_Web_Lib.Filters;
using Asp_Web_Lib.Models;
using Microsoft.AspNet.Identity.Owin;

namespace Asp_Web_Lib.Controllers
{
    [Authorize(Roles ="Worker")]
    [Culture]
    public class WorkerController : Controller
    {
        // klasa do zarządzania wszystkim czym zarządza pracownik :)

		// GET: Worker
		public ActionResult Index()
        {
            return View();
        }
    }
}