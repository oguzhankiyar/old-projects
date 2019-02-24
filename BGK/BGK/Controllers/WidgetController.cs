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
    public class WidgetController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult AboutBGK()
        {
            return PartialView();
        }
        public ActionResult MemberMenu()
        {
            return PartialView();
        }
        public ActionResult Search()
        {
            return PartialView();
        }
        public ActionResult Posts()
        {
            var posts = Db.bgk_yazi.Where(x => x.Onay == true && x.bgk_kategori.Onay == true).OrderByDescending(x => x.Okunma).Take(10).ToList();
            if (posts.Count() != 0)
                return PartialView(posts);
            return Content(null);
        }
        public ActionResult Gallery()
        {
            var gallery = Db.bgk_galeri_resim.Where(x => x.Onay == true && x.bgk_galeri.Onay == true).OrderByDescending(x => x.EklenmeTarihi).ToList();
            if (gallery.Count() != 0)
                return PartialView(gallery);
            return Content(null);
        }
        public ActionResult Poll()
        {
            int memberID = Convert.ToInt32(Session["memberID"]);
            var poll = Db.bgk_anket.Where(x => x.Onay == true && x.BitisTarihi > DateTime.Now && (memberID == 0 ? x.SadeceUye == false : true)).OrderBy(x => x.Sira).ToList();
            if (poll.Count() == 0)
                return Content(null);
            else
                return PartialView(poll.First());
        }
        public ActionResult PinnedPosts()
        {
            var posts = Db.bgk_yazi.Where(x => x.Onay == true && x.bgk_kategori.Onay == true && x.Manset == true).OrderByDescending(x => x.YazimTarihi).Take(10).ToList();
            if (posts.Count() != 0)
                return PartialView(posts);
            return Content(null);
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
