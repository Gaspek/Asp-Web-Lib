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
    [Authorize(Roles = "Admin,Worker")]
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
        [AllowAnonymous]
        // GET: Books/Details/5
        public ActionResult Details(int? id)
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
            var bookViewModel = new BookViewModel()
            {
                BookId = book.Id,
                Title = book.Title,
                Authors = string.Join(", ", book.Authors.Select(a => a.FirstName + " " + a.LastName)),
                Description = book.Description,
                CoverImage = book.CoverImage,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear.ToString("dd-MM-yyyy"),
                Publisher = book.Publisher.Name,
                Category = book.Category.Name
            };
            bookViewModel.Tags = new List<string>();
            foreach (var tag in book.Tags.Select(a => a.Name))
            {
                bookViewModel.Tags.Add(tag);
            }
            return View(bookViewModel);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            Book book = new Book();

            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");

            var allAuthors = db.Authors.ToList();
            var selectedAuthorIds = allAuthors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.FirstName + " " + a.LastName,
                Selected = false
            }).OrderBy(a => a.Text).ToList();
            ViewBag.SelectedAuthorIds = selectedAuthorIds;

            var allTags = db.Tags.ToList();
            var selectedTagIds = allTags.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name,
                Selected = false
            }).OrderBy(a => a.Text).ToList();
            ViewBag.SelectedTagIds = selectedTagIds;
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,ISBN,PublicationYear,CoverImage,PublisherId,CategoryId")] Book book, int[] SelectedAuthorIds, int[] SelectedTagIds)
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

                if (SelectedTagIds != null)
                {
                    book.Tags = new List<Tag>();
                    foreach (var tagId in SelectedTagIds)
                    {
                        var tag = db.Tags.Find(tagId);
                        if (tag != null)
                        {
                            book.Tags.Add(tag);
                        }
                    }
                }

                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name", book.PublisherId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");

            var allAuthorsReload = db.Authors.ToList();
            var selectedAuthorIdsReload = allAuthorsReload.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.FirstName + " " + a.LastName,
                Selected = SelectedAuthorIds != null && SelectedAuthorIds.Contains(a.Id)
            }).ToList();
            ViewBag.SelectedAuthorIds = selectedAuthorIdsReload;

            var allTagsReload = db.Tags.ToList();
            var selectedTagIdsReload = allTagsReload.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name,
                Selected = SelectedTagIds != null && SelectedTagIds.Contains(t.Id)
            }).ToList();
            ViewBag.SelectedTagIds = selectedTagIdsReload;

            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Book book = db.Books
                .Include(b => b.Authors)
                .Include(b => b.Publisher)
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return HttpNotFound();
            }

            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");

            var allAuthors = db.Authors.ToList();
            var selectedAuthorIds = allAuthors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.FirstName + " " + a.LastName,
                Selected = book.Authors.Any(ba => ba.Id == a.Id)
            }).ToList();
            ViewBag.SelectedAuthorIds = selectedAuthorIds;

            var allTags = db.Tags.ToList();
            var selectedTagIds = allTags.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name,
                Selected = book.Tags.Any(bt => bt.Id == t.Id)
            }).ToList();
            ViewBag.SelectedTagIds = selectedTagIds;

            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,ISBN,PublicationYear,CoverImage,PublisherId,CategoryId")] Book book, int[] SelectedAuthorIds, int[] SelectedTagIds)
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
                var bookToUpdate = db.Books
                    .Include(b => b.Authors)
                    .Include(b => b.Tags)
                    .FirstOrDefault(b => b.Id == book.Id);
                if (bookToUpdate == null)
                {
                    return HttpNotFound();
                }

                // Aktualizacja pól
                bookToUpdate.Title = book.Title;
                bookToUpdate.Description = book.Description;
                bookToUpdate.ISBN = book.ISBN;
                bookToUpdate.PublicationYear = book.PublicationYear;
                bookToUpdate.CoverImage = book.CoverImage;
                bookToUpdate.PublisherId = book.PublisherId;
                bookToUpdate.CategoryId = book.CategoryId;

                bookToUpdate.Authors.Clear();
                if (SelectedAuthorIds != null)
                {
                    foreach (var authorId in SelectedAuthorIds)
                    {
                        var author = db.Authors.Find(authorId);
                        if (author != null)
                        {
                            bookToUpdate.Authors.Add(author);
                        }
                    }
                }

                bookToUpdate.Tags.Clear();
                if (SelectedTagIds != null)
                {
                    foreach (var tagId in SelectedTagIds)
                    {
                        var tag = db.Tags.Find(tagId);
                        if (tag != null)
                        {
                            bookToUpdate.Tags.Add(tag);
                        }
                    }
                }

                db.Entry(bookToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name", book.PublisherId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", book.CategoryId);
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
