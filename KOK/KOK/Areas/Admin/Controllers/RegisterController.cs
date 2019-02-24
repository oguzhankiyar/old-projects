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
    public class RegisterController : Controller
    {
        private KOKEntities db = new KOKEntities();

        //
        // GET: /admin/Register/

        public ActionResult Index()
        {
            return View(db.basvuru.ToList());
        }

        //
        // GET: /admin/Register/Details/5

        public ActionResult Details(int id = 0)
        {
            basvuru basvuru = db.basvuru.Find(id);
            if (basvuru == null)
            {
                return HttpNotFound();
            }
            return View(basvuru);
        }

        //
        // GET: /admin/Register/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /admin/Register/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(basvuru basvuru)
        {
            if (ModelState.IsValid)
            {
                db.basvuru.Add(basvuru);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(basvuru);
        }

        //
        // GET: /admin/Register/Edit/5

        public ActionResult Edit(int id = 0)
        {
            basvuru basvuru = db.basvuru.Find(id);
            if (basvuru == null)
            {
                return HttpNotFound();
            }
            return View(basvuru);
        }

        //
        // POST: /admin/Register/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(basvuru basvuru)
        {
            if (ModelState.IsValid)
            {
                db.Entry(basvuru).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(basvuru);
        }

        //
        // GET: /admin/Register/Delete/5

        public ActionResult Delete(int id = 0)
        {
            basvuru basvuru = db.basvuru.Find(id);
            if (basvuru == null)
            {
                return HttpNotFound();
            }
            return View(basvuru);
        }

        //
        // POST: /admin/Register/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            basvuru basvuru = db.basvuru.Find(id);
            db.basvuru.Remove(basvuru);
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