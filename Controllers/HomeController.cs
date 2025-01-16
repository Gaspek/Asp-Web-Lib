using Asp_Web_Lib.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp_Web_Lib.Models;
using Asp_Web_Lib.ViewModels;


namespace Asp_Web_Lib.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var Books = db.Books.ToList();
            var model = new HomeViewModel()
            {
                Books = Books.Select(b => new BookViewModel
                {
                    Title = b.Title,
                    Description = b.Description,
                    Authors = string.Join(", ", b.Authors.Select(a => a.FirstName)),
                    CoverImage = b.CoverImage
                }).ToList()
            };
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}