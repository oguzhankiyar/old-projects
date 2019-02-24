using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using BGK.Functions;

namespace BGK.Controllers
{
    [MaintenanceControl]
    public class ActivityController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int page)
        {
            int take = 10;
            int skip = take * (page - 1);
            var activities = Db.bgk_etkinlik.OrderByDescending(x => x.BaslangicTarihi).ToList();
            ViewBag.Title = "Etkinlikler";
            ViewBag.count = activities.Count();
            ViewBag.take = take;
            return View(activities.Skip(skip).Take(take));
        }
        public ActionResult Details(int id, string seo)
        {
            var activity = Db.bgk_etkinlik.Find(id);
            if (activity == null)
                return HttpNotFound();
            return View(activity);
        }
        public ActionResult Speaker(int id, string seo)
        {
            var speaker = Db.bgk_etkinlik_konusmaci.Find(id);
            if (speaker == null)
                return HttpNotFound();
            return View(speaker);
        }
        public ActionResult Officer(int id, string seo)
        {
            var officer = Db.bgk_etkinlik_gorevli.Find(id);
            if (officer == null)
                return HttpNotFound();
            return View(officer);
        }

    }
}
