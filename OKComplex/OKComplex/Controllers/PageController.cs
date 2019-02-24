using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Models;
using OKComplex.Functions;

namespace OKComplex.Controllers
{
    [ClubSiteControl]
    public class PageController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult Detail(string seo)
        {
            var page = Db.club_page.SingleOrDefault(x => x.Seo == seo && x.IsApproval == true);
            if (page == null)
                return HttpNotFound();
            else
                return View(page);
        }

    }
}
