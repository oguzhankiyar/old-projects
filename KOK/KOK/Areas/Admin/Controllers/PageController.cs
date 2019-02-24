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
    public class PageController : Controller
    {
        private KOKEntities db = new KOKEntities();

        //
        // GET: /admin/Page/

        public ActionResult Index()
        {
            return View(db.sayfa.ToList());
        }

        //
        // GET: /admin/Page/Details/5

        public ActionResult Details(int id = 0)
        {
            sayfa sayfa = db.sayfa.Find(id);
            if (sayfa == null)
            {
                return HttpNotFound();
            }
            return View(sayfa);
        }

        //
        // GET: /admin/Page/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /admin/Page/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(sayfa sayfa)
        {
            if (ModelState.IsValid)
            {
                db.sayfa.Add(sayfa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sayfa);
        }

        //
        // GET: /admin/Page/Edit/5

        public ActionResult Edit(int id = 0)
        {
            sayfa sayfa = db.sayfa.Find(id);
            if (sayfa == null)
            {
                return HttpNotFound();
            }
            return View(sayfa);
        }

        //
        // POST: /admin/Page/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(sayfa sayfa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sayfa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sayfa);
        }

        //
        // GET: /admin/Page/Delete/5

        public ActionResult Delete(int id = 0)
        {
            sayfa sayfa = db.sayfa.Find(id);
            if (sayfa == null)
            {
                return HttpNotFound();
            }
            return View(sayfa);
        }

        //
        // POST: /admin/Page/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sayfa sayfa = db.sayfa.Find(id);
            db.sayfa.Remove(sayfa);
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