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
    public class FactoryController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int page)
        {
            int take = 10;
            int skip = take * (page - 1);
            var factories = Db.bgk_firma.OrderBy(x => x.Adi).ToList();
            ViewBag.count = factories.Count();
            ViewBag.take = take;
            ViewBag.Title = "Firmalar";
            return View(factories.Skip(skip).Take(take));
        }
        public ActionResult Details(int id, string seo)
        {
            var factory = Db.bgk_firma.Find(id);
            if (factory == null)
                return HttpNotFound();
            else
                return View(factory);
        }
    }
}
