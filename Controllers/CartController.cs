using Asp_Web_Lib.Models;
using Asp_Web_Lib.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Asp_Web_Lib.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        #region Metody pomocnicze dla cookie
        /// <summary>
        /// Zwraca nazwę cookie koszyka opartą o identyfikator zalogowanego użytkownika.
        /// Jeśli użytkownik nie jest zalogowany, zwraca "Cart".
        /// </summary>
        /// <returns>Nazwa cookie.</returns>
        private string GetCartCookieName()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Używamy identyfikatora użytkownika
                string userId = User.Identity.GetUserId();
                return "Cart_" + userId;
            }
            else
            {
                return "Cart";
            }
        }

        /// <summary>
        /// Odczytuje zawartość koszyka z cookie i zwraca listę identyfikatorów książek.
        /// </summary>
        /// <returns>Lista identyfikatorów książek.</returns>
        private List<int> GetCartFromCookie()
        {
            string cookieName = GetCartCookieName();
            HttpCookie cookie = Request.Cookies[cookieName];
            List<int> cart = new List<int>();

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                string[] parts = cookie.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    int id;
                    if (int.TryParse(part, out id))
                    {
                        cart.Add(id);
                    }
                }
            }

            return cart;
        }
        /// <summary>
        /// Zapisuje zawartość koszyka (listę identyfikatorów książek) do cookie.
        /// </summary>
        /// <param name="cart">Lista identyfikatorów książek.</param>
        private void SaveCartToCookie(List<int> cart)
        {
            string cookieName = GetCartCookieName();
            HttpCookie cookie = new HttpCookie(cookieName)
            {
                Value = string.Join(",", cart),
                Expires = DateTime.Now.AddDays(30) // cookie ważne przez 30 dni
            };
            Response.Cookies.Set(cookie);
        }
        #endregion

        // GET: Cart
        public ActionResult Index()
        {
            // Pobieramy koszyk z cookie
            List<int> cartBookIds = GetCartFromCookie();
            var viewModel = new CartViewModel();

            // Dla każdego identyfikatora książki pobieramy dane z bazy
            foreach (int bookId in cartBookIds)
            {
                // Używamy Include, aby od razu pobrać również kolekcję Copies
                var book = db.Books.Include(b => b.Copies).FirstOrDefault(b => b.Id == bookId);
                if (book != null)
                {
                    // Zakładamy, że Status.CopyStatus.Available oznacza kopię dostępną
                    int availableCopies = book.Copies.Count(c => c.Status == Status.CopyStatus.Available);

                    viewModel.Items.Add(new CartItemViewModel
                    {
                        BookId = book.Id,
                        Title = book.Title,
                        CoverImage = book.CoverImage,
                        AvailableCopies = availableCopies
                    });
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int bookId)
        {
            // Pobieramy aktualny koszyk z cookie
            List<int> cart = GetCartFromCookie();

            // Dodaj książkę do koszyka, jeśli jej tam jeszcze nie ma
            if (!cart.Contains(bookId))
            {
                cart.Add(bookId);
            }

            // Zapisz zaktualizowany koszyk do sesji
            SaveCartToCookie(cart);

            // Ustaw komunikat potwierdzający dodanie do koszyka
            TempData["Message"] = "Książka została dodana do koszyka.";

            // Przekieruj do widoku koszyka
            return RedirectToAction("Index", "Cart");
        }

        // POST: Cart/Remove/5
        [HttpPost]
        public ActionResult Remove(int bookId)
        {
            // Pobieramy koszyk z cookie
            List<int> cart = GetCartFromCookie();

            if (cart.Contains(bookId))
            {
                cart.Remove(bookId);
                SaveCartToCookie(cart);
            }
            return RedirectToAction("Index");
        }

        // POST: Cart/Loan/5
        [HttpPost]
        public ActionResult Reserve(int bookId)
        {
            string userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Pobieramy koszyk z cookie
            List<int> cart = GetCartFromCookie();

            if (cart.Contains(bookId))
            {
                cart.Remove(bookId);
                SaveCartToCookie(cart);
            }

            var copy = db.Copies.FirstOrDefault(c => c.BookId == bookId && c.Status == Status.CopyStatus.Available);

            if (copy != null)
            {

                var reservation = new Reservation()
                {
                    CopyId = copy.Id,
                    BookId = copy.BookId,
                    UserId = userId,
                    ReservationDate = DateTimeOffset.Now,
                    Status = Status.CopyStatus.Reserved
                };
                copy.Status = Status.CopyStatus.Reserved;

                user.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index", "Reservations");
            }
            else
            {
                var book = db.Books.FirstOrDefault(c => c.Id == bookId);
                if (book == null)
                {
                    return HttpNotFound("Book not found");
                }
                var queueEntry = new QueueEntry()
                {
                    BookId = bookId,
                    UserId = userId,
                    AddedAt = DateTimeOffset.Now
                };
                
                book.QueueEntries.Add(queueEntry);

                var reservation = new Reservation()
                {
                    UserId = userId,
                    BookId = bookId,
                    Status = Status.CopyStatus.InQueue
                };

                user.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index", "Reservations");

            }

            return RedirectToAction("Index");
        }

        // POST: Cart/JoinQueue/5
        [HttpPost]
        public ActionResult JoinQueue(int bookId)
        {
            // Implementacja logiki zapisu do kolejki.
            // Można tutaj dodać zapis do odpowiedniej tabeli w bazie lub inną logikę.
            TempData["Message"] = "Zapisano do kolejki dla książki o ID: " + bookId;
            return RedirectToAction("Index");
        }

        // GET: Cart/Details/5
        public ActionResult Details(int id)
        {
            // Implementacja szczegółów koszyka (opcjonalna)
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
