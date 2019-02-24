using BGK.Functions;
using BGK.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGK.Controllers
{
    [MaintenanceControl]
    public class MissionController : Controller
    {
        /*
         * bgk_gorev_uye.Durum:
         * 0 - İşlem Yapılmadı
         * 1 - Tamamlandı
         * 2 - Tamamlanmadı(Onaylandı)
         * 3 - Onaylanmadı
         * 4 - Görev İptal Edildi
         */
        private BGKEntities Db = new BGKEntities();
        public ActionResult MyMissions(int page)
        {
            int take = 10;
            int skip = take * (page - 1);
            var member = BGKFunction.GetMyAccount();
            List<bgk_gorev> missions = new List<bgk_gorev>();
            foreach (var mission in member.bgk_gorev_uye.ToList())
            {
                missions.Add(mission.bgk_gorev);
            }
            ViewBag.count = missions.Count();
            ViewBag.take = take;
            ViewBag.Title = "Görevlerim";
            ViewBag.Message = "Henüz aldığınız görev yok :(";
            return View("List", missions.Skip(skip).Take(take));
        }
        public ActionResult List(int page)
        {
            int take = 10;
            int skip = take * (page - 1);
            var member = BGKFunction.GetMyAccount();
            var missions = new List<bgk_gorev>();
            foreach (var category in member.bgk_gorev_kategori_uye.ToList())
            {
                foreach (var mission in category.bgk_gorev_kategori.bgk_gorev.ToList())
                {
                    if (member.bgk_gorev_uye.Where(x => x.GorevID == mission.Id).Count() == 0 && (member.bgk_gorev_uye.Where(x => x.bgk_gorev.Sira < mission.Sira && x.Kabul == true).Count() != 0 || Db.bgk_gorev.Where(x => x.Sira < mission.Sira).Count() == 0))
                        missions.Add(mission);
                }
            }
            ViewBag.count = missions.Count();
            ViewBag.take = take;
            ViewBag.Title = "Sıradaki Görevler";
            ViewBag.Message = "Henüz alabileceğiniz görev yok :(";
            return View(missions.Skip(skip).Take(take).ToList());
        }
        public ActionResult CreatedMissions(int page)
        {
            int take = 10;
            int skip = take * (page - 1);
            var member = BGKFunction.GetMyAccount();
            var missions = member.bgk_gorev.ToList();
            ViewBag.count = missions.Count();
            ViewBag.take = take;
            ViewBag.Title = "Oluşturduğum Görevler";
            ViewBag.Message = "Henüz oluşturduğun görev yok :(";
            return View("List", missions.Skip(skip).Take(take));
        }
        public ActionResult Categories()
        {
            ViewBag.Title = "Görev Kategorileri";
            ViewBag.Message = "Henüz kategori oluşturulmamış :(";
            return View(Db.bgk_gorev_kategori.Where(x => x.Onay == true).OrderBy(x => x.Sira).ToList());
        }
        public ActionResult SubscribedCategories()
        {
            int memberID = (int)Session["memberID"];
            ViewBag.Title = "Aboneliklerim";
            ViewBag.Message = "Henüz abone olduğun kategori yok :(";
            return View("Categories", Db.bgk_gorev_kategori.Where(x => x.Onay == true && x.bgk_gorev_kategori_uye.Where(y => y.UyeID == memberID).Count() != 0).OrderBy(x => x.Sira).ToList());
        }
        public ActionResult MyCategories()
        {
            int memberID = (int)Session["memberID"];
            ViewBag.Title = "Oluşturduğum Kategoriler";
            ViewBag.Message = "Henüz adına oluşturulan kategori yok :(";
            return View("Categories", Db.bgk_gorev_kategori.Where(x => x.OlusturanID == memberID && x.Onay == true).ToList());
        }
        public ActionResult CategoryDetails(int id)
        {
            var category = Db.bgk_gorev_kategori.Find(id);
            if (category == null)
                return HttpNotFound();
            return View(category);
        }
        public ActionResult MissionMember(int id)
        {
            int memberID = (int)Session["memberID"];
            var bgk_gorev_uye = Db.bgk_gorev_uye.Find(id);
            if (bgk_gorev_uye == null || (bgk_gorev_uye.bgk_gorev.OlusturanID != memberID && bgk_gorev_uye.UyeID != memberID))
                return HttpNotFound();
            return View(bgk_gorev_uye);
        }
        public ActionResult Details(int id)
        {
            var mission = Db.bgk_gorev.Find(id);
            if (mission == null)
                return HttpNotFound();
            return View(mission);
        }
        public ActionResult GiveMission(int id)
        {
            var category = Db.bgk_gorev_kategori.Find(id);
            if (category == null || category.OlusturanID != (int)Session["memberID"])
                return HttpNotFound();
            ViewBag.category = category;
            var first = category.bgk_gorev.OrderByDescending(x => x.Sira).FirstOrDefault();
            bgk_gorev mission = new bgk_gorev { KategoriID = id, Sira = first == null ? 0 : first.Sira };
            return View(mission);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GiveMission(bgk_gorev mission)
        {
            if (ModelState.IsValid)
            {
                mission.OlusturanID = (int)Session["memberID"];
                mission.Tip = 2;
                mission.Onay = true;
                mission.OlusturulmaTarihi = DateTime.Now;
                Db.bgk_gorev.Add(mission);
                Db.SaveChanges();
                return RedirectToAction("CreatedMissions");
            }
            return View(mission);
        }
        public ActionResult EditMission(int id)
        {
            var mission = Db.bgk_gorev.Find(id);
            if (mission == null && mission.OlusturanID != (int)Session["memberID"])
                return HttpNotFound();
            return View(mission);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMission(bgk_gorev mission)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(mission).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("details", new { id = mission.Id });
            }
            return View(mission);
        }
        public ActionResult DeleteMission(int id)
        {
            var mission = Db.bgk_gorev.Find(id);
            if (mission == null && mission.OlusturanID != (int)Session["memberID"])
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(mission);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteMission(bgk_gorev model)
        {
            bgk_gorev mission = Db.bgk_gorev.Find(model.Id);
            foreach (var member in mission.bgk_gorev_uye)
            {
                member.bgk_uye.Puan -= member.Kabul == true ? mission.Puan : 0;
                Db.bgk_gorev_uye.Remove(member);
            }
            Db.bgk_gorev.Remove(mission);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Görevi başarılı bir şekilde kaldırdınız.', function (){ window.location.href='" + Url.Action("CreatedMissions") + "'; }, 2000);</script>");
        }
        public ActionResult MissionActions()
        {
            return PartialView();
        }
        public ActionResult CancelMission(int id)
        {
            var mission = Db.bgk_gorev_uye.Find(id);
            if (mission == null || (mission.UyeID != (int)Session["memberID"] && mission.bgk_gorev.OlusturanID != (int)Session["memberID"]))
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            if (mission.UyeID == (int)Session["memberID"])
            {
                ViewBag.Title = mission.bgk_gorev.Adi + " Görevini İptal Et";
                ViewBag.Message = "<h2>Bu görevi iptal etmek istediğinizden emin misiniz?</h2><h3>İptal ettiğinizde ceza puanı alacaksınız!</h3>";
            }
            else
            {
                ViewBag.Title = mission.bgk_gorev.Adi + " Görevini Geri Al";
                ViewBag.Message = "<h2>Bu görevi geri almak istediğinizden emin misiniz?</h2>";
            }
            return PartialView("MissionActions", mission);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CancelMission(bgk_gorev_uye model)
        {
            bgk_gorev_uye mission = Db.bgk_gorev_uye.Find(model.Id);
            if (mission.UyeID == (int)Session["memberID"] && mission.bgk_gorev.Tip == 1)
            {
                BGKFunction.GetMyAccount().CezaPuani += mission.bgk_gorev.Puan;
                mission.Onay = false;
                Db.SaveChanges();
                return Content("<script>$.BGK.SuccessModal('Görevi başarılı bir şekilde iptal ettiniz.', function (){ window.location.href='" + Url.Action("MyMissions") + "'; }, 1500);</script>");
            }
            else
            {
                Db.bgk_gorev_uye.Remove(mission);
                Db.SaveChanges();
                return Content("<script>$.BGK.SuccessModal('Görevi başarılı bir şekilde iptal ettiniz.', function (){ window.location.href='" + Url.Action("CreatedMissions") + "'; }, 1500);</script>");
            }
        }
        public ActionResult ApproveMission(int id)
        {
            int memberID = (int)Session["memberID"];
            var bgk_gorev_uye = Db.bgk_gorev_uye.SingleOrDefault(x => x.GorevID == id && x.UyeID == memberID);
            if (bgk_gorev_uye == null || bgk_gorev_uye.Onay == true)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            ViewBag.Title = "Görevi Onayla";
            ViewBag.Message = "<h3>Bu görevi onaylamak istediğinizden emin misiniz?</h3><h4>Zamanında tamamlamadığınızda ceza puanı alırsınız!</h4>";
            return PartialView("MissionActions", bgk_gorev_uye);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveMission(bgk_gorev_uye model)
        {
            int memberID = (int)Session["memberID"];
            var bgk_gorev_uye = Db.bgk_gorev_uye.SingleOrDefault(x => x.GorevID == model.Id && x.UyeID == memberID);
            bgk_gorev_uye.Onay = true;
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Görevi başarılı bir şekilde onayladınız.', function (){ window.location.reload(); }, 1500);</script>");
        }
        public ActionResult UnapproveMission(int id)
        {
            int memberID = (int)Session["memberID"];
            var bgk_gorev_uye = Db.bgk_gorev_uye.SingleOrDefault(x => x.GorevID == id && x.UyeID == memberID);
            if (bgk_gorev_uye == null || bgk_gorev_uye.Onay == true)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            ViewBag.Title = "Görevi Geri Çevir";
            ViewBag.Message = "<h3>Bu görevi onaylamak istediğinizden emin misiniz?</h3><h4>Zorunlu görevi almadığınızdan ceza puanı alacaksınız!</h4>";
            return PartialView("MissionActions", bgk_gorev_uye);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnapproveMission(bgk_gorev_uye model)
        {
            int memberID = (int)Session["memberID"];
            var bgk_gorev_uye = Db.bgk_gorev_uye.SingleOrDefault(x => x.GorevID == model.Id && x.UyeID == memberID);
            bgk_gorev_uye.Onay = false;
            BGKFunction.GetMember(bgk_gorev_uye.UyeID).CezaPuani += bgk_gorev_uye.bgk_gorev.Puan;
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Görevi başarılı bir şekilde geri çevirdiniz.', function (){ window.location.reload(); }, 1500);</script>");
        }
        public ActionResult AcceptMission(int id)
        {
            var bgk_gorev_uye = Db.bgk_gorev_uye.Find(id);
            if (bgk_gorev_uye == null || bgk_gorev_uye.Tamamlandi == false || bgk_gorev_uye.Kabul != null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            ViewBag.Title = "Görevi Kabul Et";
            ViewBag.Message = "<h3>Bu görevi kabul etmek istediğinizden emin misiniz?</h3>";
            return PartialView("MissionActions", bgk_gorev_uye);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptMission(bgk_gorev_uye model)
        {
            var bgk_gorev_uye = Db.bgk_gorev_uye.Find(model.Id);
            bgk_gorev_uye.Kabul = true;
            BGKFunction.GetMember(bgk_gorev_uye.UyeID).Puan += bgk_gorev_uye.bgk_gorev.Puan;
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Görevi başarıyla kabul ettiniz.', function (){ window.location.reload(); }, 1500);</script>");
        }
        public ActionResult RevertMission(int id)
        {
            var bgk_gorev_uye = Db.bgk_gorev_uye.Find(id);
            if (bgk_gorev_uye == null || bgk_gorev_uye.Tamamlandi == false || bgk_gorev_uye.Kabul != null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            ViewBag.Title = "Görevi Reddet";
            ViewBag.PartialView = "<h3>Bu görevi reddetmek istediğinizden emin misiniz?</h3>";
            return PartialView("MissionActions", bgk_gorev_uye);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RevertMission(bgk_gorev_uye model)
        {
            var bgk_gorev_uye = Db.bgk_gorev_uye.Find(model.Id);
            bgk_gorev_uye.Kabul = false;
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Görevi başarıyla reddettiniz.', function (){ window.location.reload(); }, 1500);</script>");
        }
        public ActionResult CompleteMission(int id)
        {
            int memberID = (int)Session["memberID"];
            var bgk_gorev_uye = Db.bgk_gorev_uye.SingleOrDefault(x => x.GorevID == id && x.UyeID == memberID);
            if (bgk_gorev_uye == null)
                return HttpNotFound();
            return View(bgk_gorev_uye);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteMission(bgk_gorev_uye model)
        {
            if (ModelState.IsValid)
            {
                int memberID = (int)Session["memberID"];
                var bgk_gorev_uye = Db.bgk_gorev_uye.SingleOrDefault(x => x.GorevID == model.Id && x.UyeID == memberID);
                bgk_gorev_uye.Rapor = model.Rapor;
                bgk_gorev_uye.DosyaID = model.DosyaID;
                bgk_gorev_uye.BitisTarihi = DateTime.Now;
                bgk_gorev_uye.Tamamlandi = true;
                Db.Entry(bgk_gorev_uye).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("mymissions");
            }
            return View(model);
        }
        public ActionResult TakeMission(int id)
        {
            int memberID = (int)Session["memberID"];
            var mission = Db.bgk_gorev.Find(id);
            if (mission == null || mission.bgk_gorev_uye.SingleOrDefault(x => x.UyeID == memberID) != null)
                return HttpNotFound();
            bool prevMission = true;
            bool isSubscribed = false;
            var category = mission.KategoriID != null ? mission.bgk_gorev_kategori : null;
            if (category != null && category.bgk_gorev_kategori_uye.SingleOrDefault(x => x.UyeID == memberID) != null)
            {
                isSubscribed = true;
                foreach (var prev in category.bgk_gorev.Where(x => x.Sira < mission.Sira).ToList())
                {
                    if (prev.bgk_gorev_uye.SingleOrDefault(x => x.UyeID == memberID && x.Kabul == true) == null)
                        prevMission = false;
                }
            }
            if (mission.OlusturanID == memberID)
                return Content("<script>$.BGK.ErrorModal('Kendi oluşturduğun görevi alamazsın!');</script>");
            else if (!isSubscribed)
                return Content("<script>$.BGK.ErrorModal('Bu görevi alabilmeniz için bu kategoriye abone olmalısınız!');</script>");
            else if (!prevMission)
                return Content("<script>$.BGK.ErrorModal('Bu görevi alabilmeniz için kategorinin önceki görevlerinizin tamamlanmış ve kabul edilmiş olması gerekiyor!');</script>");
            return PartialView(mission);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult TakeMission(bgk_gorev mission)
        {
            var new_mission = new bgk_gorev_uye();
            mission = Db.bgk_gorev.Find(mission.Id);
            new_mission.GorevID = mission.Id;
            new_mission.UyeID = (int)Session["memberID"];
            new_mission.BaslangicTarihi = Convert.ToDateTime(Request.Form["BaslangicTarihi"]);
            new_mission.BitisTarihi = Convert.ToDateTime(Request.Form["BitisTarihi"]);
            new_mission.Onay = true;
            new_mission.Tamamlandi = false;
            Db.bgk_gorev_uye.Add(new_mission);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Görevi başarıyla aldınız.', function (){ window.location.href='" + Url.Action("Details", new { id = mission.Id }) + "'; }, 1500);</script>");
        }
        public ActionResult Subscribe(int categoryID)
        {
            var category = Db.bgk_gorev_kategori.Find(categoryID);
            var member = BGKFunction.GetMyAccount();
            if (category == null || category.bgk_gorev_kategori_uye.SingleOrDefault(x => x.UyeID == member.Id) != null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            else if (category.OlusturanID == member.Id)
                return Content("<script>$.BGK.ErrorModal('Kendi oluşturduğun kategoriye abone olamazsın!');</script>");
            else if (category.PuanSiniri != null && category.PuanSiniri > member.Puan)
                return Content("<script>$.BGK.ErrorModal('Bu kategoriye abone olabilmeniz için en az " + category.PuanSiniri + " puanınız olmalı! Sizin şuanda " + member.Puan + " puanınız bulunmaktadır.');</script>");
            ViewBag.category = category; 
            var mission_member = new bgk_gorev_kategori_uye() { UyeID = member.Id, KategoriID = categoryID }; 
            return PartialView(mission_member);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Subscribe(bgk_gorev_kategori_uye member)
        {
            member.BaslamaTarihi = DateTime.Now;
            Db.bgk_gorev_kategori_uye.Add(member);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Kategoriye başarıyla abone oldunuz.', function (){ window.location.reload(); }, 1500);</script>");
        }
        public ActionResult Unsubscribe(int categoryID)
        {
            var category = Db.bgk_gorev_kategori.Find(categoryID);
            int memberID = (int)Session["memberID"];
            if (category == null || category.bgk_gorev_kategori_uye.SingleOrDefault(x => x.UyeID == memberID) == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            var member = Db.bgk_gorev_kategori_uye.SingleOrDefault(x => x.UyeID == memberID && x.KategoriID == categoryID);
            return PartialView(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unsubscribe(bgk_gorev_kategori_uye model)
        {
            var member = Db.bgk_gorev_kategori_uye.Find(model.Id);
            int point = 0;
            foreach (var mission in member.bgk_gorev_kategori.bgk_gorev)
            {
                var missionmember = mission.bgk_gorev_uye.SingleOrDefault(x => x.UyeID == member.UyeID);
                if (missionmember != null)
                {
                    point += missionmember.Kabul == true ? mission.Puan : 0;
                    Db.bgk_gorev_uye.Remove(missionmember);
                }
            }
            BGKFunction.GetMyAccount().Puan -= point;
            Db.bgk_gorev_kategori_uye.Remove(member);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Kategori aboneliğiniz başarıyla kaldırıldı.', function (){ window.location.reload(); }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
