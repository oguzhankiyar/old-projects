using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using System.Data;
using BGK.Functions;

namespace BGK.Areas.Admin.Controllers
{
    [Controls("ConfigManagement")]
    public class ConfigController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var config = Db.bgk_ayar.OrderBy(x => x.Id).ToList();
            ViewBag.take = take;
            ViewBag.count = config.Count();
            return View(config.Skip(skip).Take(take));
        }
        public ActionResult Details(int num = 0)
        {
            bgk_ayar config = Db.bgk_ayar.Find(num);
            if (config == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(config);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_ayar config)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_ayar.Add(config);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(config);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_ayar config = Db.bgk_ayar.Find(num);
            if (config == null)
                return HttpNotFound();
            return View(config);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_ayar config)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(config).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(config);
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_ayar config = Db.bgk_ayar.Find(num);
            if (config == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = config.Adi, Message = "Bu ayarı silmek istediğinizden emin misiniz?<br />Bu ayarın siteden kullanılmadığından emin olun!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_ayar config = Db.bgk_ayar.Find(model.Id);
            Db.bgk_ayar.Remove(config);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('İşlem başarıyla silindi.', function (){ window.location.href='" + Url.Action("index") +"'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
