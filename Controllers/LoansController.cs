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
using Microsoft.AspNet.Identity;

namespace Asp_Web_Lib.Controllers
{
    [Culture]
    public class LoansController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Loans
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var loans = db.Loans
                .Include(r => r.Copy.Book)
                .Include(r => r.User)
                .Where(r => r.UserId == userId);
            var viewModel = new LoanViewModel();

            foreach (var loan in loans.ToList())
            {
                var viewItem = new LoanItemViewModel()
                {
                    BookId = loan.Copy.BookId,
                    Title = loan.Copy.Book.Title,
                    CoverImage = loan.Copy.Book.CoverImage,
                    LoanId = loan.Id,
                    DueDate = loan.DueDate,
                    LoanDate = loan.LoanDate,
                    LoanStatus = Status.CopyStatus.Borrowed
                };
                if (loan.ReturnDate.HasValue)
                {
                    viewItem.ReturnDate = loan.ReturnDate;
                    viewItem.LoanStatus = Status.CopyStatus.Returned;
                }
                viewModel.Items.Add(viewItem);
            }

            return View(viewModel);
        }

        // GET: Loans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // GET: Loans/Create
        public ActionResult Create()
        {
            ViewBag.CopyId = new SelectList(db.Copies, "Id", "ShelfLocation");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CopyId,UserId,LoanDate,DueDate,ReturnDate")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Loans.Add(loan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CopyId = new SelectList(db.Copies, "Id", "ShelfLocation", loan.CopyId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", loan.UserId);
            return View(loan);
        }

        // GET: Loans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            ViewBag.CopyId = new SelectList(db.Copies, "Id", "ShelfLocation", loan.CopyId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", loan.UserId);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CopyId,UserId,LoanDate,DueDate,ReturnDate")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CopyId = new SelectList(db.Copies, "Id", "ShelfLocation", loan.CopyId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", loan.UserId);
            return View(loan);
        }

        // GET: Loans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Loan loan = db.Loans.Find(id);
            db.Loans.Remove(loan);
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
