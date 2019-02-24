using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OKPort.Models;

namespace OKPort.Areas.Admin.Controllers
{
    public class PageController : Controller
    {
        private OKDbEntities db = new OKDbEntities();

        // GET: Admin/Page
        public ActionResult Index()
        {
            var pages = db.Pages.Include(p => p.Theme).Include(p => p.User);
            return View(pages.ToList());
        }

        // GET: Admin/Page/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // GET: Admin/Page/Create
        public ActionResult Create()
        {
            ViewBag.ThemeID = new SelectList(db.Themes, "ID", "Name");
            ViewBag.OwnerID = new SelectList(db.Users, "ID", "Email");
            return View();
        }

        // POST: Admin/Page/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,ImageURL,OwnerID,ShortURL,ThemeID,CreationDate,IsApproval,IndexTemplate")] Page page)
        {
            if (ModelState.IsValid)
            {
                db.Pages.Add(page);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ThemeID = new SelectList(db.Themes, "ID", "Name", page.ThemeID);
            ViewBag.OwnerID = new SelectList(db.Users, "ID", "Email", page.OwnerID);
            return View(page);
        }

        // GET: Admin/Page/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            ViewBag.ThemeID = new SelectList(db.Themes, "ID", "Name", page.ThemeID);
            ViewBag.OwnerID = new SelectList(db.Users, "ID", "Email", page.OwnerID);
            return View(page);
        }

        // POST: Admin/Page/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,ImageURL,OwnerID,ShortURL,ThemeID,CreationDate,IsApproval,IndexTemplate")] Page page)
        {
            if (ModelState.IsValid)
            {
                db.Entry(page).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ThemeID = new SelectList(db.Themes, "ID", "Name", page.ThemeID);
            ViewBag.OwnerID = new SelectList(db.Users, "ID", "Email", page.OwnerID);
            return View(page);
        }

        // GET: Admin/Page/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: Admin/Page/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Page page = db.Pages.Find(id);
            db.Pages.Remove(page);
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
