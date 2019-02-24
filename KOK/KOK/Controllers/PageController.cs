using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KOK.Models;

namespace KOK.Controllers
{
    public class PageController : Controller
    {
        private KOKEntities Db = new KOKEntities();
        public ActionResult Index(string seo)
        {
            var page = Db.sayfa.SingleOrDefault(x => x.Seo == seo && x.Onay == true);
            if (page == null)
                return HttpNotFound();
            return View(page);
        }

    }
}
