using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asp_Web_Lib.Models;

namespace Asp_Web_Lib.Controllers
{
    public class LimitsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Limits
        public ActionResult Index()
        {
            return View(db.Limits.ToList());
        }

        // GET: Limits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Limits limits = db.Limits.Find(id);
            if (limits == null)
            {
                return HttpNotFound();
            }
            return View(limits);
        }

        // GET: Limits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Limits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdLimits,MaxBorrowedBooks,MaxWaitingBooks,MaxExtensionNumber,ExtensionDays,LoanAmount")] Limits limits)
        {
            if (ModelState.IsValid)
            {
                db.Limits.Add(limits);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(limits);
        }

        // GET: Limits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Limits limits = db.Limits.Find(id);
            if (limits == null)
            {
                return HttpNotFound();
            }
            return View(limits);
        }

        // POST: Limits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdLimits,MaxBorrowedBooks,MaxWaitingBooks,MaxExtensionNumber,ExtensionDays,LoanAmount")] Limits limits)
        {
            if (ModelState.IsValid)
            {
                db.Entry(limits).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(limits);
        }

        // GET: Limits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Limits limits = db.Limits.Find(id);
            if (limits == null)
            {
                return HttpNotFound();
            }
            return View(limits);
        }

        // POST: Limits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Limits limits = db.Limits.Find(id);
            db.Limits.Remove(limits);
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
