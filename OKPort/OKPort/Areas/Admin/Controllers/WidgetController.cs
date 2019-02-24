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
    public class WidgetController : Controller
    {
        private OKDbEntities db = new OKDbEntities();

        // GET: Admin/Widget
        public ActionResult Index()
        {
            var widgets = db.Widgets.Include(w => w.User);
            return View(widgets.ToList());
        }

        // GET: Admin/Widget/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Widget widget = db.Widgets.Find(id);
            if (widget == null)
            {
                return HttpNotFound();
            }
            return View(widget);
        }

        // GET: Admin/Widget/Create
        public ActionResult Create()
        {
            ViewBag.OwnerID = new SelectList(db.Users, "ID", "Email");
            return View();
        }

        // POST: Admin/Widget/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Content,OwnerID,CreationDate,IsApproval")] Widget widget)
        {
            if (ModelState.IsValid)
            {
                db.Widgets.Add(widget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerID = new SelectList(db.Users, "ID", "Email", widget.OwnerID);
            return View(widget);
        }

        // GET: Admin/Widget/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Widget widget = db.Widgets.Find(id);
            if (widget == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerID = new SelectList(db.Users, "ID", "Email", widget.OwnerID);
            return View(widget);
        }

        // POST: Admin/Widget/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Content,OwnerID,CreationDate,IsApproval")] Widget widget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(widget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerID = new SelectList(db.Users, "ID", "Email", widget.OwnerID);
            return View(widget);
        }

        // GET: Admin/Widget/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Widget widget = db.Widgets.Find(id);
            if (widget == null)
            {
                return HttpNotFound();
            }
            return View(widget);
        }

        // POST: Admin/Widget/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Widget widget = db.Widgets.Find(id);
            db.Widgets.Remove(widget);
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
