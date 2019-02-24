using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KOK.Models;

namespace KOK.Controllers
{
    public class HomeController : Controller
    {
        private KOKEntities Db = new KOKEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Menu()
        {
            return PartialView(Db.link.Where(x => x.AnaMenu == true && x.Onay == true).OrderBy(x => x.Sira).ToList());
        }
        public ActionResult LeftMenu()
        {
            return PartialView(Db.link.Where(x => x.AnaMenu == false && x.Onay == true).OrderBy(x => x.Sira).ToList());
        }
        public ActionResult Notices()
        {
            return PartialView(Db.duyuru.Where(x => x.Onay == true && x.Manset == true).OrderBy(x => x.Sira).ToList());
        }
        public ActionResult MiniNotices()
        {
            return PartialView(Db.duyuru.Where(x => x.Onay == true).OrderBy(x => x.Sira).ToList());
        }
        public ActionResult NoticeDetails(int id)
        {
            var notice = Db.duyuru.Find(id);
            if (notice == null || notice.Onay == false)
                return HttpNotFound();
            return View(notice);
        }
    }
}
