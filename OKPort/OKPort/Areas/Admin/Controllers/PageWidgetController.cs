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
    public class PageWidgetController : Controller
    {
        private OKDbEntities db = new OKDbEntities();

        // GET: Admin/PageWidget
        public ActionResult Index()
        {
            var pageWidgets = db.PageWidgets.Include(p => p.Section).Include(p => p.Widget);
            return View(pageWidgets.ToList());
        }

        // GET: Admin/PageWidget/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PageWidget pageWidget = db.PageWidgets.Find(id);
            if (pageWidget == null)
            {
                return HttpNotFound();
            }
            return View(pageWidget);
        }

        // GET: Admin/PageWidget/Create
        public ActionResult Create()
        {
            ViewBag.PageID = new SelectList(db.Pages, "ID", "Name");
            ViewBag.SectionID = new SelectList(db.Sections, "ID", "Name");
            ViewBag.WidgetID = new SelectList(db.Widgets, "ID", "Title");
            return View();
        }

        // POST: Admin/PageWidget/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PageID,SectionID,WidgetID,Position,Region,Sort")] PageWidget pageWidget)
        {
            if (ModelState.IsValid)
            {
                db.PageWidgets.Add(pageWidget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PageID = new SelectList(db.Pages, "ID", "Name", pageWidget.PageID);
            ViewBag.SectionID = new SelectList(db.Sections, "ID", "Name", pageWidget.SectionID);
            ViewBag.WidgetID = new SelectList(db.Widgets, "ID", "Title", pageWidget.WidgetID);
            return View(pageWidget);
        }

        // GET: Admin/PageWidget/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PageWidget pageWidget = db.PageWidgets.Find(id);
            if (pageWidget == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageID = new SelectList(db.Pages, "ID", "Name", pageWidget.PageID);
            ViewBag.SectionID = new SelectList(db.Sections, "ID", "Name", pageWidget.SectionID);
            ViewBag.WidgetID = new SelectList(db.Widgets, "ID", "Title", pageWidget.WidgetID);
            return View(pageWidget);
        }

        // POST: Admin/PageWidget/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PageID,SectionID,WidgetID,Position,Region,Sort")] PageWidget pageWidget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pageWidget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PageID = new SelectList(db.Pages, "ID", "Name", pageWidget.PageID);
            ViewBag.SectionID = new SelectList(db.Sections, "ID", "Name", pageWidget.SectionID);
            ViewBag.WidgetID = new SelectList(db.Widgets, "ID", "Title", pageWidget.WidgetID);
            return View(pageWidget);
        }

        // GET: Admin/PageWidget/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PageWidget pageWidget = db.PageWidgets.Find(id);
            if (pageWidget == null)
            {
                return HttpNotFound();
            }
            return View(pageWidget);
        }

        // POST: Admin/PageWidget/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PageWidget pageWidget = db.PageWidgets.Find(id);
            db.PageWidgets.Remove(pageWidget);
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
