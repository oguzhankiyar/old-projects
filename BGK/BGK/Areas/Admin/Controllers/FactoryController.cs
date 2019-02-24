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
    [Controls("FactoryManagement")]
    public class FactoryController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var factory = Db.bgk_firma;
            ViewBag.count = factory.Count();
            ViewBag.take = 10;
            return View(factory.OrderBy(x => x.Adi).Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_firma bgk_firma = Db.bgk_firma.Find(num);
            if (bgk_firma == null)
                return HttpNotFound();
            return View(bgk_firma);
        }
        public ActionResult Create()
        {
            ViewBag.UyeID = new SelectList(Db.bgk_uye, "Id", "AdSoyad");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_firma bgk_firma)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_firma.Add(bgk_firma);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UyeID = new SelectList(Db.bgk_uye, "Id", "AdSoyad");
            return View(bgk_firma);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_firma bgk_firma = Db.bgk_firma.Find(num);
            if (bgk_firma == null)
                return HttpNotFound();
            ViewBag.UyeID = new SelectList(Db.bgk_uye, "Id", "AdSoyad", bgk_firma.UyeID);
            return View(bgk_firma);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_firma bgk_firma)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(bgk_firma).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UyeID = new SelectList(Db.bgk_uye, "Id", "AdSoyad", bgk_firma.UyeID);
            return View(bgk_firma);
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_firma bgk_firma = Db.bgk_firma.Find(num);
            if (bgk_firma == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = bgk_firma.Adi, Message = "Bu firmayı ve bu firmaya ait üyeliği, kategorileri silmek istediğinize emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_firma bgk_firma = Db.bgk_firma.Find(model.Id);
            Db.bgk_dosya.Remove(bgk_firma.bgk_dosya);
            foreach (var category in bgk_firma.bgk_uye.bgk_gorev_kategori)
            {
                Db.bgk_gorev_kategori.Remove(category);
            }
            Db.bgk_uye.Remove(bgk_firma.bgk_uye);
            Db.bgk_firma.Remove(bgk_firma);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Firma başarıyla silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}