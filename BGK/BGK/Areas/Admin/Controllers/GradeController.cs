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
    [Controls("GradeManagement")]
    public class GradeController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index()
        {
            return View(Db.bgk_seviye.OrderBy(x => x.AltSinir).ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_seviye bgk_seviye)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_seviye.Add(bgk_seviye);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bgk_seviye);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_seviye bgk_seviye = Db.bgk_seviye.Find(num);
            if (bgk_seviye == null)
                return HttpNotFound();
            return View(bgk_seviye);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_seviye bgk_seviye)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(bgk_seviye).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bgk_seviye);
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_seviye bgk_seviye = Db.bgk_seviye.Find(num);
            if (bgk_seviye == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = bgk_seviye.Adi, Message = "Bu seviyeyi silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_seviye bgk_seviye = Db.bgk_seviye.Find(model.Id);
            Db.bgk_seviye.Remove(bgk_seviye);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Seviye başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}