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
    [Controls("DocumentManagement")]
    public class DocumentController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            int categoryID = Convert.ToInt32(Request.QueryString["categoryID"]);
            var bgk_dokuman = Request.QueryString["categoryID"] == null ? Db.bgk_dokuman : Db.bgk_dokuman.Where(x => x.KategoriID == categoryID);
            ViewBag.count = bgk_dokuman.Count();
            ViewBag.take = take;
            return View(bgk_dokuman.OrderByDescending(x => x.EklenmeTarihi).Skip(skip).Take(take));
        }
        public ActionResult Details(int num = 0)
        {
            bgk_dokuman bgk_dokuman = Db.bgk_dokuman.Find(num);
            if (bgk_dokuman == null)
                return HttpNotFound();
            return View(bgk_dokuman);
        }
        public ActionResult Create()
        {
            ViewBag.KategoriID = new SelectList(Db.bgk_dokuman_kategori, "Id", "Adi");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_dokuman bgk_dokuman)
        {
            if (ModelState.IsValid)
            {
                bgk_dokuman.Seo = bgk_dokuman.Adi.ConvertSeo();
                bgk_dokuman.EklenmeTarihi = DateTime.Now;
                bgk_dokuman.UyeID = (int)Session["memberID"];
                Db.bgk_dokuman.Add(bgk_dokuman);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KategoriID = new SelectList(Db.bgk_dokuman_kategori, "Id", "Adi", bgk_dokuman.KategoriID);
            return View(bgk_dokuman);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_dokuman bgk_dokuman = Db.bgk_dokuman.Find(num);
            if (bgk_dokuman == null)
                return HttpNotFound();
            ViewBag.KategoriID = new SelectList(Db.bgk_dokuman_kategori, "Id", "Adi", bgk_dokuman.KategoriID);
            return View(bgk_dokuman);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_dokuman bgk_dokuman)
        {
            if (ModelState.IsValid)
            {
                bgk_dokuman.Seo = bgk_dokuman.Adi.ConvertSeo();
                Db.Entry(bgk_dokuman).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DosyaID = new SelectList(Db.bgk_dosya, "Id", "Aciklama", bgk_dokuman.DosyaID);
            ViewBag.KategoriID = new SelectList(Db.bgk_dokuman_kategori, "Id", "Adi", bgk_dokuman.KategoriID);
            ViewBag.UyeID = new SelectList(Db.bgk_uye, "Id", "AdSoyad", bgk_dokuman.UyeID);
            return View(bgk_dokuman);
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_dokuman bgk_dokuman = Db.bgk_dokuman.Find(num);
            if (bgk_dokuman == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = bgk_dokuman.Adi, Message = "Bu dökümanı silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_dokuman bgk_dokuman = Db.bgk_dokuman.Find(model.Id);
            BGKFunction.RemoveUploadFile(bgk_dokuman.bgk_dosya);
            Db.bgk_dokuman.Remove(bgk_dokuman);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Döküman başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}