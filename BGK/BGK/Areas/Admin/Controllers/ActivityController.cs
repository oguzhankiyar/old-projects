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
    [Controls("ActivityManagement")]
    public class ActivityController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var activity = Db.bgk_etkinlik;
            ViewBag.count = activity.Count();
            ViewBag.take = 10;
            return View(activity.OrderByDescending(x => x.BaslangicTarihi).Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_etkinlik bgk_etkinlik = Db.bgk_etkinlik.Find(num);
            if (bgk_etkinlik == null)
                return HttpNotFound();
            return View(bgk_etkinlik);
        }
        public ActionResult Create()
        {
            ViewBag.FirmaID = new SelectList(Db.bgk_firma, "Id", "Adi");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_etkinlik bgk_etkinlik)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_etkinlik.Add(bgk_etkinlik);
                Db.SaveChanges();
                return RedirectToAction("Details", new { num = bgk_etkinlik.Id });
            }
            ViewBag.FirmaID = new SelectList(Db.bgk_firma, "Id", "Adi", bgk_etkinlik.FirmaID);
            return View(bgk_etkinlik);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_etkinlik bgk_etkinlik = Db.bgk_etkinlik.Find(num);
            if (bgk_etkinlik == null)
                return HttpNotFound();
            ViewBag.FirmaID = new SelectList(Db.bgk_firma, "Id", "Adi", bgk_etkinlik.FirmaID);
            return View(bgk_etkinlik);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_etkinlik bgk_etkinlik)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(bgk_etkinlik).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Details", new { num = bgk_etkinlik.Id });
            }
            ViewBag.FirmaID = new SelectList(Db.bgk_firma, "Id", "Adi", bgk_etkinlik.FirmaID);
            return View(bgk_etkinlik);
        }
        public ActionResult SpeakerDetails(int num)
        {
            var speaker = Db.bgk_etkinlik_konusmaci.Find(num);
            if (speaker == null || speaker.bgk_etkinlik == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(speaker);
        }
        public ActionResult AddSpeaker(int num)
        {
            var activity = Db.bgk_etkinlik.Find(num);
            if (activity == null)
                return HttpNotFound();
            bgk_etkinlik_konusmaci speaker = new bgk_etkinlik_konusmaci() { EtkinlikID = num, bgk_etkinlik = activity };
            return View(speaker);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSpeaker(bgk_etkinlik_konusmaci bgk_etkinlik_konusmaci)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_etkinlik_konusmaci.Add(bgk_etkinlik_konusmaci);
                Db.SaveChanges();
                return RedirectToAction("Details", new { id = bgk_etkinlik_konusmaci.EtkinlikID });
            }
            return View(bgk_etkinlik_konusmaci);
        }
        public ActionResult EditSpeaker(int num)
        {
            var speaker = Db.bgk_etkinlik_konusmaci.Find(num);
            if (speaker == null || speaker.bgk_etkinlik == null)
                return HttpNotFound();
            return View(speaker);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSpeaker(bgk_etkinlik_konusmaci bgk_etkinlik_konusmaci)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(bgk_etkinlik_konusmaci).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Details", new { num = bgk_etkinlik_konusmaci.EtkinlikID });
            }
            return View(bgk_etkinlik_konusmaci);
        }
        public ActionResult DeleteSpeaker(int num)
        {
            var speaker = Db.bgk_etkinlik_konusmaci.Find(num);
            if (speaker == null || speaker.bgk_etkinlik == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = speaker.AdSoyad, Message = "Bu konuşmacıyı silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSpeaker(delete_action model)
        {
            var speaker = Db.bgk_etkinlik_konusmaci.Find(model.Id);
            int activityID = speaker.EtkinlikID;
            Db.bgk_etkinlik_konusmaci.Remove(speaker);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Konuşmacı başarıyla silindi.', function (){ window.location.href='" + Url.Action("details", new { num = activityID }) + "'; }, 1500);</script>");
        }
        public ActionResult OfficerDetails(int num)
        {
            var officer = Db.bgk_etkinlik_gorevli.Find(num);
            if (officer == null || officer.bgk_etkinlik == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(officer);
        }
        public ActionResult AddOfficer(int num)
        {
            var activity = Db.bgk_etkinlik.Find(num);
            if (activity == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            ViewBag.UyeID = new SelectList(Db.bgk_uye, "Id", "AdSoyad");
            ViewBag.GorevID = new SelectList(Db.bgk_gorev, "Id", "Adi");
            bgk_etkinlik_gorevli officer = new bgk_etkinlik_gorevli() { EtkinlikID = num, bgk_etkinlik = activity };
            return PartialView(officer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOfficer(bgk_etkinlik_gorevli bgk_etkinlik_gorevli)
        {
            if (ModelState.IsValid)
            {
                int activityID = bgk_etkinlik_gorevli.EtkinlikID;
                Db.bgk_etkinlik_gorevli.Add(bgk_etkinlik_gorevli);
                Db.SaveChanges();
                return Content("<script>$.BGK.SuccessModal('Görevli başarılı bir şekilde eklendi.', function (){ window.location.reload(); }, 1500);</script>");
            }
            ViewBag.UyeID = new SelectList(Db.bgk_uye, "Id", "AdSoyad");
            ViewBag.GorevID = new SelectList(Db.bgk_gorev, "Id", "Adi");
            return Content("<font color=\"red\">Boş alan bıraktınız!</font>");
        }
        public ActionResult DeleteOfficer(int num)
        {
            var officer = Db.bgk_etkinlik_gorevli.Find(num);
            if (officer == null || officer.bgk_etkinlik == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = officer.bgk_uye.AdSoyad, Message = "Bu görevliyi silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOfficer(delete_action model)
        {
            var officer = Db.bgk_etkinlik_gorevli.Find(model.Id);
            int activityID = officer.EtkinlikID;
            Db.bgk_etkinlik_gorevli.Remove(officer);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Görevli başarıyla silindi.', function (){ window.location.href='" + Url.Action("details", new { num = activityID }) + "'; }, 1500);</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_etkinlik bgk_etkinlik = Db.bgk_etkinlik.Find(num);
            if (bgk_etkinlik == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = bgk_etkinlik.Adi, Message = "Bu etkinliği silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_etkinlik bgk_etkinlik = Db.bgk_etkinlik.Find(model.Id);
            BGKFunction.DeleteActivity(bgk_etkinlik);
            Db.bgk_dosya.Remove(bgk_etkinlik.bgk_dosya);
            Db.bgk_etkinlik.Remove(bgk_etkinlik);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Etkinlik başarılı bir şekilde silindi.', function () { window.location.href='" + Url.Action("index") + "' });</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}