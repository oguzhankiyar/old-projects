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
    public class ImageController : Controller
    {
        private KOKEntities db = new KOKEntities();

        //
        // GET: /admin/Image/

        public ActionResult Index()
        {
            return View(db.resim.ToList());
        }

        //
        // GET: /admin/Image/Details/5

        public ActionResult Details(int id = 0)
        {
            resim resim = db.resim.Find(id);
            if (resim == null)
            {
                return HttpNotFound();
            }
            return View(resim);
        }

        //
        // GET: /admin/Image/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /admin/Image/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(resim resim)
        {
            if (ModelState.IsValid)
            {
                db.resim.Add(resim);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(resim);
        }

        //
        // GET: /admin/Image/Edit/5

        public ActionResult Edit(int id = 0)
        {
            resim resim = db.resim.Find(id);
            if (resim == null)
            {
                return HttpNotFound();
            }
            return View(resim);
        }

        //
        // POST: /admin/Image/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(resim resim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(resim);
        }

        //
        // GET: /admin/Image/Delete/5

        public ActionResult Delete(int id = 0)
        {
            resim resim = db.resim.Find(id);
            if (resim == null)
            {
                return HttpNotFound();
            }
            return View(resim);
        }

        //
        // POST: /admin/Image/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            resim resim = db.resim.Find(id);
            db.resim.Remove(resim);
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