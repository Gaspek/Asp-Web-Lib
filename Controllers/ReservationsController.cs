﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asp_Web_Lib.Filters;
using Asp_Web_Lib.Models;
using Asp_Web_Lib.Services;
using Asp_Web_Lib.ViewModels;
using Microsoft.AspNet.Identity;

namespace Asp_Web_Lib.Controllers
{
    [Culture]
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
            var userId = User.Identity.GetUserId();
            var reservation = db.Reservations.Include(c => c.Copy).Include(c => c.User).FirstOrDefault(c => c.Id == reservationId);
            if (reservation == null)
            {
                return HttpNotFound("Błąd przy usuwaniu rezerwacji");
            }

            if (reservation.Status == Status.CopyStatus.Reserved || reservation.Status == Status.CopyStatus.ReadyForPickUp)
            {
                var bookService = new BookService(db);
                MvcApplication._bookService.ReturnCopy(reservation.Copy.Id);
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

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            ViewBag.CopyId = new SelectList(db.Copies, "Id", "ShelfLocation");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CopyId,UserId,ReservationDate,Status")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CopyId = new SelectList(db.Copies, "Id", "ShelfLocation", reservation.CopyId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CopyId = new SelectList(db.Copies, "Id", "ShelfLocation", reservation.CopyId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", reservation.UserId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CopyId,UserId,ReservationDate,Status")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CopyId = new SelectList(db.Copies, "Id", "ShelfLocation", reservation.CopyId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
