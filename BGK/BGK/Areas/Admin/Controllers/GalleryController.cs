using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using BGK.Functions;

namespace BGK.Areas.Admin.Controllers
{
    [Controls("GalleryManagement")]
    public class GalleryController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var gallery = Db.bgk_galeri;
            ViewBag.count = gallery.Count();
            ViewBag.take = take;
            return View(gallery.OrderByDescending(x => x.OlusturulmaTarihi).Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            var gallery = Db.bgk_galeri.Find(num);
            if (gallery == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(gallery);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_galeri gallery)
        {
            if (ModelState.IsValid)
            {
                gallery.Seo = gallery.Adi.ConvertSeo();
                gallery.OlusturulmaTarihi = DateTime.Now;
                Db.bgk_galeri.Add(gallery);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gallery);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_galeri gallery = Db.bgk_galeri.Find(num);
            if (gallery == null)
                return HttpNotFound();
            return View(gallery);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_galeri gallery)
        {
            if (ModelState.IsValid)
            {
                gallery.Seo = gallery.Adi.ConvertSeo();
                Db.Entry(gallery).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gallery);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            bgk_galeri gallery = Db.bgk_galeri.Find(num);
            if (gallery != null)
            {
                gallery.Onay = gallery.Onay == true ? false : true;
                result = gallery.Onay == true ? "Galeri başarıyla onaylandı." : "Galerinin onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script>$.BGK.SuccessModal('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_galeri gallery = Db.bgk_galeri.Find(num);
            if (gallery == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = gallery.Adi, Message = "Bu galeriyi ve bu galeriye ait tüm resimleri silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_galeri gallery = Db.bgk_galeri.Find(model.Id);
            foreach (var image in gallery.bgk_galeri_resim.ToList())
            {
                Db.bgk_galeri_resim.Remove(image);
            }
            Db.bgk_galeri.Remove(gallery);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Galeri başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}