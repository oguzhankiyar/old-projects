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
    public class AdminController : Controller
    {
        private KOKEntities db = new KOKEntities();

        //
        // GET: /admin/Admin/

        public ActionResult Index()
        {
            return View(db.yonetici.ToList());
        }

        //
        // GET: /admin/Admin/Details/5

        public ActionResult Details(int id = 0)
        {
            yonetici yonetici = db.yonetici.Find(id);
            if (yonetici == null)
            {
                return HttpNotFound();
            }
            return View(yonetici);
        }

        //
        // GET: /admin/Admin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /admin/Admin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(yonetici yonetici)
        {
            if (ModelState.IsValid)
            {
                db.yonetici.Add(yonetici);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(yonetici);
        }

        //
        // GET: /admin/Admin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            yonetici yonetici = db.yonetici.Find(id);
            if (yonetici == null)
            {
                return HttpNotFound();
            }
            return View(yonetici);
        }

        //
        // POST: /admin/Admin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(yonetici yonetici)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yonetici).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yonetici);
        }

        //
        // GET: /admin/Admin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            yonetici yonetici = db.yonetici.Find(id);
            if (yonetici == null)
            {
                return HttpNotFound();
            }
            return View(yonetici);
        }

        //
        // POST: /admin/Admin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            yonetici yonetici = db.yonetici.Find(id);
            db.yonetici.Remove(yonetici);
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