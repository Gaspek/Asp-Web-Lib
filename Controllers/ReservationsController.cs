using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asp_Web_Lib.Filters;
using Asp_Web_Lib.Models;
using Asp_Web_Lib.Services;
using Asp_Web_Lib.ViewModels;
using Microsoft.AspNet.Identity;
using static Asp_Web_Lib.MvcApplication;

namespace Asp_Web_Lib.Controllers
{
    [Culture]
    [Authorize]
    public class ReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservations
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var reservations = db.Reservations
                .Include(r => r.Copy)
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.UserId == userId);
            var viewModel = new ReservationViewModel();

            foreach (var reservation in reservations.ToList())
            {
                var viewItem = new ReservationItemViewModel()
                {
                    BookId = reservation.Book.Id,
                    Title = reservation.Book.Title,
                    CoverImage = reservation.Book.CoverImage,
                    ReservationId = reservation.Id,
                    ReservationStatus = reservation.Status
                };
                if (reservation.Status == Status.CopyStatus.ReadyForPickUp)
                {
                    viewItem.AcceptanceDate = reservation.AcceptanceDate;
                }
                viewModel.Items.Add(viewItem);
            }

            return View(viewModel);
        }

        //POST:Reservations/Remove/5
        [HttpPost]
        public ActionResult Remove(int bookId, int reservationId)
        {
            var reservation = db.Reservations.Include(c => c.Copy).Include(c => c.User).FirstOrDefault(c => c.Id == reservationId);
            if (reservation == null)
            {
                return HttpNotFound("Błąd przy usuwaniu rezerwacji");
            }

            var userId = reservation.UserId;
            if (userId == null)
            {
                db.Reservations.Remove(reservation);
                db.SaveChanges();
                return HttpNotFound("Błąd wczytywania użytkownika");
            }

            if (reservation.Status == Status.CopyStatus.Reserved || reservation.Status == Status.CopyStatus.ReadyForPickUp)
            {
                _bookService.ReturnCopy(reservation.Copy.Id);
            }
            else
            {
                var book = db.Books.Include(c => c.QueueEntries).FirstOrDefault(c => c.Id == bookId);
                if (book == null)
                {
                    return HttpNotFound("Błąd przy usuwaniu rezerwacji");
                }

                var entry = db.QueueEntries.FirstOrDefault(c => c.UserId == userId);
                if (entry != null)
                {
                    db.QueueEntries.Remove(entry);
                    db.SaveChanges();
                }
            }

            db.Reservations.Remove(reservation);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Reservations/Manage
        [Authorize(Roles = "Worker,Admin")]
        public ActionResult Manage()
        {
            var reservations = db.Reservations
                .Include(r => r.Copy)
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.Status == Status.CopyStatus.Reserved || r.Status == Status.CopyStatus.ReadyForPickUp);
            var viewModel = new ReservationViewModel();

            foreach (var reservation in reservations.ToList())
            {
                var viewItem = new ReservationItemViewModel()
                {
                    BookId = reservation.Book.Id,
                    Title = reservation.Book.Title,
                    CoverImage = reservation.Book.CoverImage,
                    ReservationId = reservation.Id,
                    ReservationStatus = reservation.Status,
                    UserId = reservation.UserId,
                    UserName = reservation.User.UserName
                };
                if (reservation.Status == Status.CopyStatus.ReadyForPickUp)
                {
                    viewItem.AcceptanceDate = reservation.AcceptanceDate;
                }
                viewModel.Items.Add(viewItem);
            }

            return View(viewModel);
        }

        //POST: Reservation/Accept/5
        [HttpPost]
        [Authorize(Roles = "Worker,Admin")]
        public ActionResult Accept(int reservationId)
        {
            var reservation = db.Reservations.Include(c => c.Copy).Include(c => c.User).FirstOrDefault(c => c.Id == reservationId);
            if (reservation == null)
            {
                return HttpNotFound("Błąd 404 nie znaleziono rezerwacji");
            }

            reservation.Status = Status.CopyStatus.ReadyForPickUp;
            reservation.AcceptanceDate = DateTimeOffset.Now;

            db.Entry(reservation).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Manage");
        }

        //POST: Reservation/Accept/5
        [HttpPost]
        [Authorize(Roles = "Worker,Admin")]
        public ActionResult ConfirmPickUp(int reservationId)
        {
            var reservation = db.Reservations.Include(c => c.Copy).Include(c => c.User).FirstOrDefault(c => c.Id == reservationId);
            if (reservation == null)
            {
                return HttpNotFound("Błąd 404 nie znaleziono rezerwacji");
            }

            var userId = reservation.UserId;
            if (userId == null)
            {
                db.Reservations.Remove(reservation);
                db.SaveChanges();
                return HttpNotFound("Błąd wczytywania użytkownika");
            }

            var days = db.Limits.Select(c => c.MaxExtensionNumber).FirstOrDefault();
            if (days == 0)
            {
                days = 30;
            }
            var loan = new Loan()
            {
                CopyId = reservation.CopyId,
                UserId = userId,
                LoanDate = DateTimeOffset.Now,
                DueDate = DateTimeOffset.Now.AddDays(days),
            };

            db.Reservations.Remove(reservation);
            db.Loans.Add(loan);
            db.SaveChanges();

            return RedirectToAction("Manage");
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
