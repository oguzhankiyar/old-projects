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
    public class LinkController : Controller
    {
        private KOKEntities db = new KOKEntities();

        //
        // GET: /admin/Link/

        public ActionResult Index()
        {
            return View(db.link.ToList());
        }

        //
        // GET: /admin/Link/Details/5

        public ActionResult Details(int id = 0)
        {
            link link = db.link.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        //
        // GET: /admin/Link/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /admin/Link/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(link link)
        {
            if (ModelState.IsValid)
            {
                db.link.Add(link);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(link);
        }

        //
        // GET: /admin/Link/Edit/5

        public ActionResult Edit(int id = 0)
        {
            link link = db.link.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        //
        // POST: /admin/Link/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(link link)
        {
            if (ModelState.IsValid)
            {
                db.Entry(link).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(link);
        }

        //
        // GET: /admin/Link/Delete/5

        public ActionResult Delete(int id = 0)
        {
            link link = db.link.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        //
        // POST: /admin/Link/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            link link = db.link.Find(id);
            db.link.Remove(link);
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