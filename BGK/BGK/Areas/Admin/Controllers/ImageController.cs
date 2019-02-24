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
    public class ImageController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 18;
            int skip = take * (num - 1);
            int galleryID = Convert.ToInt32(Request.QueryString["galleryID"]);
            var images = string.IsNullOrEmpty(Request.QueryString["galleryID"]) ? Db.bgk_galeri_resim.OrderByDescending(x => x.EklenmeTarihi) : Db.bgk_galeri_resim.Where(x => x.GaleriID == galleryID).OrderBy(x => x.Sira);
            ViewBag.count = images.Count();
            ViewBag.take = take;
            ViewBag.Gallery = Db.bgk_galeri.Find(galleryID);
            return View(images.Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_galeri_resim bgk_galeri_resim = Db.bgk_galeri_resim.Find(num);
            if (bgk_galeri_resim == null)
            {
                return HttpNotFound();
            }
            return View(bgk_galeri_resim);
        }
        public ActionResult Create()
        {
            bgk_galeri_resim image = new bgk_galeri_resim();
            int galleryID = Convert.ToInt32(Request.QueryString["galleryID"]);
            ViewBag.GaleriID = new SelectList(Db.bgk_galeri, "Id", "Adi", galleryID);
            return View(image);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_galeri_resim bgk_galeri_resim)
        {
            if (ModelState.IsValid)
            {
                bgk_galeri_resim.EklenmeTarihi = DateTime.Now;
                Db.bgk_galeri_resim.Add(bgk_galeri_resim);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GaleriID = new SelectList(Db.bgk_galeri, "Id", "Adi", bgk_galeri_resim.GaleriID);
            return View(bgk_galeri_resim);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_galeri_resim bgk_galeri_resim = Db.bgk_galeri_resim.Find(num);
            if (bgk_galeri_resim == null)
                return HttpNotFound();
            ViewBag.GaleriID = new SelectList(Db.bgk_galeri, "Id", "Adi", bgk_galeri_resim.GaleriID);
            return View(bgk_galeri_resim);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_galeri_resim bgk_galeri_resim)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(bgk_galeri_resim).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GaleriID = new SelectList(Db.bgk_galeri, "Id", "Adi", bgk_galeri_resim.GaleriID);
            return View(bgk_galeri_resim);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            bgk_galeri_resim image = Db.bgk_galeri_resim.Find(num);
            if (image != null)
            {
                image.Onay = !image.Onay;
                result = image.Onay == true ? "Resim başarıyla onaylandı." : "Resmin onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script>$.BGK.SuccessModal('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_galeri_resim bgk_galeri_resim = Db.bgk_galeri_resim.Find(num);
            if (bgk_galeri_resim == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = "Resim Sil", Message = "Bu resmi silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_galeri_resim bgk_galeri_resim = Db.bgk_galeri_resim.Find(model.Id);
            Db.bgk_galeri_resim.Remove(bgk_galeri_resim);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Resim başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}