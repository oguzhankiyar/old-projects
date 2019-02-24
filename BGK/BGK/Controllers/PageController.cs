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
    public class PageController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Details(string seo)
        {
            var page = Db.bgk_sayfa.SingleOrDefault(x => x.Seo == seo && x.Onay == true);
            if (page == null)
                return HttpNotFound();
            else
                return View(page);
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
