using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KOK.Models;

namespace KOK.Areas.Admin.Controllers
{
    public class NoticeController : Controller
    {
        private KOKEntities db = new KOKEntities();

        //
        // GET: /admin/Notice/

        public ActionResult Index()
        {
            return View(db.duyuru.ToList());
        }

        //
        // GET: /admin/Notice/Details/5

        public ActionResult Details(int id = 0)
        {
            duyuru duyuru = db.duyuru.Find(id);
            if (duyuru == null)
            {
                return HttpNotFound();
            }
            return View(duyuru);
        }

        //
        // GET: /admin/Notice/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /admin/Notice/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(duyuru duyuru)
        {
            if (ModelState.IsValid)
            {
                db.duyuru.Add(duyuru);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(duyuru);
        }

        //
        // GET: /admin/Notice/Edit/5

        public ActionResult Edit(int id = 0)
        {
            duyuru duyuru = db.duyuru.Find(id);
            if (duyuru == null)
            {
                return HttpNotFound();
            }
            return View(duyuru);
        }

        //
        // POST: /admin/Notice/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(duyuru duyuru)
        {
            if (ModelState.IsValid)
            {
                db.Entry(duyuru).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(duyuru);
        }

        //
        // GET: /admin/Notice/Delete/5

        public ActionResult Delete(int id = 0)
        {
            duyuru duyuru = db.duyuru.Find(id);
            if (duyuru == null)
            {
                return HttpNotFound();
            }
            return View(duyuru);
        }

        //
        // POST: /admin/Notice/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            duyuru duyuru = db.duyuru.Find(id);
            db.duyuru.Remove(duyuru);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}