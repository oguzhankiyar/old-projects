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
    [Controls("NotificationManagement")]
    public class NotificationController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 20;
            int skip = take * (num - 1);
            int adminID = Convert.ToInt32(BGKFunction.GetConfig("adminID"));
            var bgk_bildirim = Db.bgk_bildirim.Where(x => x.UyeID == adminID).OrderByDescending(x => x.Tarih);
            ViewBag.count = bgk_bildirim.Count();
            ViewBag.take = take;
            return View(bgk_bildirim.Skip(skip).Take(take));
        }
        public ActionResult Create(int num = 0)
        {
            ViewBag.UyeID = new SelectList(Db.bgk_uye, "Id", "AdSoyad", num);
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_bildirim model)
        {
            if (ModelState.IsValid)
            {
                model.Tarih = DateTime.Now;
                model.Okundu = false;
                Db.bgk_bildirim.Add(model);
                Db.SaveChanges();
                return Content("<script>$.BGK.SuccessModal('Bildirim başarılı bir şekilde eklendi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
            }
            return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            bgk_bildirim notification = Db.bgk_bildirim.Find(num);
            if (notification != null)
            {
                notification.Okundu = !notification.Okundu;
                result = notification.Okundu == true ? "Bildirim okundu olarak işaretlendi." : "Bildirim okunmadı olarak işaretlendi.";
                Db.SaveChanges();
            }
            return Content("<script>$.BGK.SuccessModal('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_bildirim bgk_bildirim = Db.bgk_bildirim.Find(num);
            if (bgk_bildirim == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = "Bildirim Sil", Message = "Bu bildirimi silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_bildirim bgk_bildirim = Db.bgk_bildirim.Find(model.Id);
            Db.bgk_bildirim.Remove(bgk_bildirim);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Bildirim başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }

        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}