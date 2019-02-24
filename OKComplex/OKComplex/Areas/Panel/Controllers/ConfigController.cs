using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Models;
using OKComplex.Functions;
using System.Data;

namespace OKComplex.Areas.Panel.Controllers
{
    [PanelControl]
    public class ConfigController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult Index()
        {
            return View(Db.club_config.ToList());
        }
        public ActionResult Details(int num = 0)
        {
            club_config config = Db.club_config.Find(num);
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
        public ActionResult Create(club_config config)
        {
            if (ModelState.IsValid)
            {
                Db.club_config.Add(config);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(config);
        }
        public ActionResult Edit(int num = 0)
        {
            club_config config = Db.club_config.Find(num);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }
        [HttpPost]
        public ActionResult Edit(club_config config)
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
            club_config config = Db.club_config.Find(num);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            club_config config = Db.club_config.Find(num);
            Db.club_config.Remove(config);
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
