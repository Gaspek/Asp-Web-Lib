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
using Asp_Web_Lib.Services;
using Asp_Web_Lib.ViewModels;
using Microsoft.AspNet.Identity;
using static Asp_Web_Lib.MvcApplication;

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
                .Where(r => r.UserId == userId && r.IsHistory == false);
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

        // GET: Loans
        public ActionResult History()
        {
            var userId = User.Identity.GetUserId();
            var loans = db.Loans
                .Include(r => r.Copy.Book)
                .Include(r => r.User)
                .Where(r => r.UserId == userId && r.IsHistory);
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

        // GET: Loans/Manage
        [Authorize(Roles = "Worker,Admin")]
        public ActionResult Manage()
        {
            var loans = db.Loans
                .Include(r => r.Copy.Book)
                .Include(r => r.User)
                .Where(r => r.ReturnDate.HasValue == false);
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
                viewModel.Items.Add(viewItem);
            }

            return View(viewModel);
        }

        //POST:Loans/Return/5
        [HttpPost]
        [Authorize(Roles = "Worker,Admin")]
        public ActionResult Return(int loanId)
        {
            var loan = db.Loans.Include(c => c.Copy).Include(c => c.User).FirstOrDefault(c => c.Id == loanId);
            if (loan == null)
            {
                return HttpNotFound("Błąd przy zwrocie książki");
            }

            var userId = loan.UserId;
            if (userId == null)
            {
                return HttpNotFound("Błąd wczytywania użytkownika dla książki");
            }

            loan.ReturnDate = DateTimeOffset.Now;
            loan.IsHistory = true;

            _bookService.ReturnCopy(loan.Copy.Id);

            db.Entry(loan).State = EntityState.Modified;
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
