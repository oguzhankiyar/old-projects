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
    [Controls("RoleManagement")]
    public class RoleController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index()
        {
            return View(Db.bgk_yetki.OrderBy(x => x.Id).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_yetki bgk_yetki = Db.bgk_yetki.Find(num);
            if (bgk_yetki == null)
                return HttpNotFound();
            return View(bgk_yetki);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_yetki bgk_yetki)
        {
            if (ModelState.IsValid)
            {
                if (Db.bgk_yetki.SingleOrDefault(x => x.Kod == bgk_yetki.Kod) != null)
                    ModelState.AddModelError("Kod", "Bu kod daha önceden eklenmiş.");
                else
                {
                    Db.bgk_yetki.Add(bgk_yetki);
                    Db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(bgk_yetki);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_yetki bgk_yetki = Db.bgk_yetki.Find(num);
            if (bgk_yetki == null)
                return HttpNotFound();
            return View(bgk_yetki);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_yetki bgk_yetki)
        {
            if (ModelState.IsValid)
            {
                if (Db.bgk_yetki.SingleOrDefault(x => x.Kod == bgk_yetki.Kod && x.Id != bgk_yetki.Id) != null)
                    ModelState.AddModelError("Kod", "Bu kod daha önceden eklenmiş.");
                else
                {
                    Db.Entry(bgk_yetki).State = EntityState.Modified;
                    int code = Convert.ToInt32(Request.Form["OldCode"]);
                    foreach (var member in Db.bgk_uye.Where(x => x.Yetki == code))
                    {
                        member.Yetki = (int)bgk_yetki.Kod;
                    }
                    Db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(bgk_yetki);
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_yetki bgk_yetki = Db.bgk_yetki.Find(num);
            if (bgk_yetki == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = bgk_yetki.Adi, Message = "Bu yetkiyi silmek istediğinizden emin misiniz?<br />Bu yetkiye sahip üye olmadığından emin olun!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_yetki bgk_yetki = Db.bgk_yetki.Find(model.Id);
            Db.bgk_yetki.Remove(bgk_yetki);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Yetki başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}