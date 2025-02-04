﻿using Asp_Web_Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp_Web_Lib.Filters;

namespace Asp_Web_Lib.Controllers
{
    [Culture]
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Search
        public ActionResult Index()
        {
            ViewBag.BooksList = new LinkedList<SelectListItem>();

            return View();
        }
        public JsonResult GetBooks()
        {
            var books = db.Books
                .Include(b => b.Authors) // Załaduj autorów
                .Include(b => b.Tags) //Załaduj tagi
                .OrderBy(b => b.Title)
                .ToList() // Pobierz dane do pamięci, aby móc użyć `string.Join`
                .Select(b => new
                {
                    ISBN = b.ISBN,
                    Author = b.Authors != null && b.Authors.Any()
                        ? string.Join(", ", b.Authors.Select(a => a.FirstName + " " + a.LastName))
                        : "Brak autora",
                    Title = b.Title,
                    Id = b.Id,
                    CoverImg = b.CoverImage,
                    Tags = b.Tags.Select(t => t.Name).ToList()
                })
                .ToList();

            return Json(books, JsonRequestBehavior.AllowGet);
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