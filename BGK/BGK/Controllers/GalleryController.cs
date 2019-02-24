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
    public class GalleryController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int page)
        {
            int take = 7;
            int skip = take * (page - 1);
            var gallery = Db.bgk_galeri.Where(x => x.Onay == true).OrderByDescending(x => x.OlusturulmaTarihi);
            ViewBag.Title = "Galeri";
            ViewBag.count = gallery.Count();
            ViewBag.take = take;
            return View(gallery.Skip(skip).Take(take).ToList());
        }
        public ActionResult Images(string seo, int page)
        {
            var gallery = Db.bgk_galeri.SingleOrDefault(x => x.Seo == seo && x.Onay == true);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            else
            {
                var images = gallery.bgk_galeri_resim.Where(x => x.Onay == true).OrderBy(x => x.Sira);
                int take = 7;
                int skip = take * (page - 1);
                ViewBag.Title = gallery.Adi + " Galerisindeki Resimler";
                ViewBag.count = images.Count();
                ViewBag.take = take;
                ViewBag.seo = seo;
                return View(images.Skip(skip).Take(take).ToList());
            }
        }
        public ActionResult View(string seo, int id)
        {
            var image = Db.bgk_galeri_resim.Find(id);
            if (image == null || image.Onay == false || image.bgk_galeri.Seo != seo)
                return HttpNotFound();
            else
                return View(image);
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
