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
    [Controls("NoteManagement")]
    public class NoteController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var bgk_not = Db.bgk_not.OrderByDescending(x => x.Tarih);
            ViewBag.count = bgk_not.Count();
            ViewBag.take = take;
            return View(bgk_not.Skip(skip).Take(take));
        }
        public ActionResult Details(int num = 0)
        {
            bgk_not bgk_not = Db.bgk_not.Find(num);
            if (bgk_not == null)
                return HttpNotFound();
            return View(bgk_not);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_not bgk_not)
        {
            if (ModelState.IsValid)
            {
                bgk_not.UyeID = (int)Session["memberID"];
                bgk_not.Tarih = DateTime.Now;
                Db.bgk_not.Add(bgk_not);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bgk_not);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_not bgk_not = Db.bgk_not.Find(num);
            if (bgk_not == null)
                return HttpNotFound();
            return View(bgk_not);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_not bgk_not)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(bgk_not).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UyeID = new SelectList(Db.bgk_uye, "Id", "AdSoyad", bgk_not.UyeID);
            return View(bgk_not);
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_not bgk_not = Db.bgk_not.Find(num);
            if (bgk_not == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = "Not Sil", Message = "Bu notu silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_not bgk_not = Db.bgk_not.Find(model.Id);
            Db.bgk_not.Remove(bgk_not);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Not başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}