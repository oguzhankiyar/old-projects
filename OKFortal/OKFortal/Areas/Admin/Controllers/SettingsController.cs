using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKFortal.Models;
using OKFortal.Functions;

namespace OKFortal.Areas.Admin.Controllers
{
    [AdminControl]
    public class SettingsController : Controller
    {
        private OKDbEntities Db = new OKDbEntities();
        public ActionResult Index()
        {
            return View(Db.config.ToList());
        }
        public ActionResult Details(int num = 0)
        {
            config config = Db.config.Find(num);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(config config)
        {
            if (ModelState.IsValid)
            {
                Db.config.Add(config);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(config);
        }
        public ActionResult Edit(int num = 0)
        {
            config config = Db.config.Find(num);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }
        [HttpPost]
        public ActionResult Edit(config config)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(config).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(config);
        }
        public ActionResult Delete(int num = 0)
        {
            config config = Db.config.Find(num);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            config config = Db.config.Find(num);
            Db.config.Remove(config);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}