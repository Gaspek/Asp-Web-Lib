using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asp_Web_Lib.Filters;
using Asp_Web_Lib.Models;
using Asp_Web_Lib.ViewModels;
using Microsoft.SqlServer.Server;


namespace Asp_Web_Lib.Controllers
{
    [Authorize(Roles = "Admin")]
    [Culture]
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.Publisher);
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            var bookViewModel = new BookViewModel()
            {
                Title = book.Title,
                Authors = string.Join(", ", book.Authors.Select(a => a.FirstName + " " + a.LastName)),
                Description = book.Description,
                CoverImage = book.CoverImage,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear.ToString("dd-MM-yyyy"),
                Publisher = book.Publisher.Name,
                Category = book.Category.Name
            };
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(bookViewModel);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name");
            var authors = db.Authors.Select(a => new
            {
                Id = a.Id,
                FullName = a.FirstName + " " + a.LastName
            }).ToList();
            ViewBag.SelectedAuthorIds = new MultiSelectList(authors, "Id", "FullName");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            var category = db.Categories.Select(a => new
            {
                Id = a.Id,
                Name = a.Name
            }).ToList().OrderBy(a => a.Name);
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,ISBN,PublicationYear,CoverImage,PublisherId,CategoryId")] Book book, int[] SelectedAuthorIds)
        {
            if (db.Books.Any(b => b.Title.ToLower().Trim() == book.Title.ToLower().Trim()))
            {
                ModelState.AddModelError("","Książka o takim tytule już istnieje");
            }
            if (db.Books.Any(b => b.ISBN.ToLower().Trim() == book.ISBN.ToLower().Trim()))
            {
                ModelState.AddModelError("", "Książka o takim nr ISBN już istnieje");
            }
            if (ModelState.IsValid)
            {
                if (SelectedAuthorIds != null)
                {
                    book.Authors = new List<Author>();
                    foreach (var authorId in SelectedAuthorIds)
                    {
                        var author = db.Authors.Find(authorId);
                        if (author != null)
                        {
                            book.Authors.Add(author);
                        }
                    }
                }
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name", book.PublisherId);
            var authors = db.Authors.Select(a => new
            {
                Id = a.Id,
                FullName = a.FirstName + " " + a.LastName
            }).ToList();
            ViewBag.SelectedAuthorIds = new MultiSelectList(authors, "Id", "FullName", SelectedAuthorIds);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            var category = db.Categories.Select(a => new
            {
                Id = a.Id,
                Name = a.Name
            }).ToList().OrderBy(a => a.Name);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name");
            var authors = db.Authors.Select(a => new
            {
                Id = a.Id,
                FullName = a.FirstName + " " + a.LastName
            }).ToList();
            ViewBag.SelectedAuthorIds = new MultiSelectList(authors, "Id", "FullName");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            var category = db.Categories.Select(a => new
            {
                Id = a.Id,
                Name = a.Name
            }).ToList().OrderBy(a => a.Name);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name", book.PublisherId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,ISBN,PublicationYear,CoverImage,PublisherId,CategoryId")] Book book, int[] SelectedAuthorIds)
        {
            // Pobranie książki i sprawdzenie tytułu oraz ISBN
            var existingBooks = db.Books
                .Where(b => b.Id == book.Id || b.Title.Trim().ToLower() == book.Title.Trim().ToLower() || b.ISBN.Trim().ToLower() == book.ISBN.Trim().ToLower())
                .ToList();
            if (!existingBooks.Any())
            {
                return HttpNotFound();
            }
            // Sprawdzamy unikalność tytułu i ISBN
            if (existingBooks.Any(b => b.Title.ToLower().Trim() == book.Title.ToLower().Trim() && b.Id != book.Id))
            {
                ModelState.AddModelError("Title", "Książka o takim tytule już istnieje");
            }
            if (existingBooks.Any(b => b.ISBN.ToLower().Trim() == book.ISBN.ToLower().Trim() && b.Id != book.Id))
            {
                ModelState.AddModelError("ISBN", "Książka o takim nr ISBN już istnieje");
            }
            if (ModelState.IsValid)
            {
                // Pobranie książki do aktualizacji
                var bookToUpdate = db.Books.Find(book.Id);
                if (bookToUpdate == null)
                {
                    return HttpNotFound();
                }

                // Aktualizacja pól
                bookToUpdate.Title = book.Title;
                bookToUpdate.Description = book.Description;
                bookToUpdate.ISBN = book.ISBN;
                bookToUpdate.PublicationYear = book.PublicationYear;
                bookToUpdate.PublisherId = book.PublisherId;
                if (SelectedAuthorIds != null)
                {
                    bookToUpdate.Authors = new List<Author>();
                    foreach (var authorId in SelectedAuthorIds)
                    {
                        var author = db.Authors.Find(authorId);
                        if (author != null)
                        {
                            bookToUpdate.Authors.Add(author);
                        }
                    }
                }

                db.Entry(bookToUpdate).CurrentValues.SetValues(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name", book.PublisherId);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
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
