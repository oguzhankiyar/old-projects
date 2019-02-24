using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using BGK.Functions;

namespace BGK.Controllers
{
    [MaintenanceControl]
    public class GroupController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int page)
        {
            int take = 10;
            int skip = take * (page - 1);
            var group = Db.bgk_grup.OrderBy(x => x.Adi);
            ViewBag.count = group.Count();
            ViewBag.take = take;
            return View(group.Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int id, string seo)
        {
            var group = Db.bgk_grup.Find(id);
            if (group == null)
                return HttpNotFound();
            return View(group);
        }
        public ActionResult MemberDetails(int id, string seo)
        {
            bgk_grup_uye bgk_grup_uye = Db.bgk_grup_uye.Find(id);
            var group = bgk_grup_uye.bgk_grup;
            if (bgk_grup_uye == null || group.bgk_grup_uye.SingleOrDefault(x => x.UyeID == (int)Session["memberID"] && x.GrupID == group.Id && x.Tip == 1) == null)
                return HttpNotFound();
            return View(bgk_grup_uye);
        }
        public ActionResult AddMember(int id)
        {
            var group = Db.bgk_grup.Find(id);
            if (group == null || group.bgk_grup_uye.SingleOrDefault(x => x.UyeID == (int)Session["memberID"] && x.Tip == 1) == null)
                return HttpNotFound();
            ViewBag.group = group;
            bgk_grup_uye member = new bgk_grup_uye() { GrupID = id, BaslangicTarihi = DateTime.Now, Aktif = true };
            ViewBag.UyeID = new SelectList(Db.bgk_uye.Where(x => x.bgk_grup_uye.Where(y => y.GrupID == id).Count() == 0), "Id", "AdSoyad");
            return View(member);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddMember(bgk_grup_uye model)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_grup_uye.Add(model);
                Db.SaveChanges();
                return RedirectToAction("Details", new { id = model.GrupID });
            }
            ViewBag.UyeID = new SelectList(Db.bgk_uye, "Id", "AdSoyad");
            return View(model);
        }
        public ActionResult EditMember(int id)
        {
            var member = Db.bgk_grup_uye.Find(id);
            var group = member.bgk_grup;
            if (member == null || group.bgk_grup_uye.SingleOrDefault(x => x.UyeID == (int)Session["memberID"] && x.Tip == 1) == null)
                return HttpNotFound();
            return View(member);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditMember(bgk_grup_uye model)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_grup_uye.Add(model);
                Db.SaveChanges();
                return RedirectToAction("Details", new { id = model.GrupID });
            }
            return View(model);
        }
        public ActionResult DeleteMember(int id)
        {
            var member = Db.bgk_grup_uye.Find(id);
            var group = member.bgk_grup;
            if (member == null || group.bgk_grup_uye.SingleOrDefault(x => x.UyeID == (int)Session["memberID"] && x.Tip == 1) == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMember(bgk_grup_uye model)
        {
            bgk_grup_uye bgk_grup_uye = Db.bgk_grup_uye.Find(model.Id);
            int groupID = bgk_grup_uye.GrupID;
            Db.bgk_grup_uye.Remove(bgk_grup_uye);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Üyeliği başarıyla kaldırdınız.', function (){ window.location.href='" + Url.Action("details", new { id = groupID }) + "'; }, 1500);</script>");
        }
        public ActionResult SendMessage(int id)
        {
            var group = Db.bgk_grup.Find(id);
            if (group == null || group.bgk_grup_uye.SingleOrDefault(x => x.UyeID == (int)Session["memberID"] && x.Tip == 1) == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            bgk_mesaj message = new bgk_mesaj() { GonderenID = group.Id };
            return PartialView(message);
        }
        [HttpPost]
        public ActionResult SendMessage(bgk_mesaj message)
        {
            var group = Db.bgk_grup.Find(message.GonderenID);
            if (string.IsNullOrEmpty(message.Mesaj))
                return Content("<font color=\"red\">Mesaj alanını boş bıraktınız!</font>");
            foreach (var member in group.bgk_grup_uye)
            {
                if (member.UyeID != (int)Session["memberID"])
                {
                    message.Mesaj += "<br /><br /><i><b>Not:</b> Bu mesaj <a href=\"" + Url.Action("details", new { id = group.Id }) + "\">" + group.Adi + "</a> grubu adına <a href=\"" + Url.Action("Details", "Member", new { id = (int)Session["memberID"] }) + "\">" + BGKFunction.GetMyAccount().AdSoyad + "</a> tarafından gönderilmiştir.</i>";
                    message.AliciID = member.UyeID;
                    message.GonderenID = Convert.ToInt32(BGKFunction.GetConfig("adminID"));
                    message.Okundu = false;
                    message.Tip = 2;
                    message.YazimTarihi = DateTime.Now;
                    Db.bgk_mesaj.Add(message);
                    Db.SaveChanges();
                }
            }
            return Content("<script>$.BGK.SuccessModal('Mesajınız başarıyla gönderilmiştir.');</script>");
        }
        public ActionResult GiveMission(int id)
        {
            var group = Db.bgk_grup.Find(id);
            if (group == null)
                return HttpNotFound();
            ViewBag.group = group;
            bgk_gorev mission = new bgk_gorev { GrupID = id };
            return View(mission);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GiveMission(bgk_gorev mission)
        {
            if (ModelState.IsValid)
            {
                mission.OlusturanID = (int)Session["memberID"];
                mission.OlusturulmaTarihi = DateTime.Now;
                mission.Tip = 1;
                Db.bgk_gorev.Add(mission);
                var group = Db.bgk_grup.Find(mission.GrupID);
                foreach (var member in group.bgk_grup_uye.ToList())
                {
                    string cevap = Request.Form["member_" + member.UyeID.ToString()];
                    if (Request.Form["member_" + member.UyeID.ToString()] != null && Request.Form["member_" + member.UyeID.ToString()] != "false")
                    {
                        bgk_gorev_uye bgk_gorev_uye = new bgk_gorev_uye();
                        bgk_gorev_uye.GorevID = mission.Id;
                        bgk_gorev_uye.UyeID = member.UyeID;
                        bgk_gorev_uye.Tamamlandi = false;
                        bgk_gorev_uye.BaslangicTarihi = DateTime.Now;
                        bgk_gorev_uye.BitisTarihi = (DateTime)mission.BitisTarihi;
                        bgk_gorev_uye.Onay = null;
                        Db.bgk_gorev_uye.Add(bgk_gorev_uye);
                        Db.SaveChanges();
                    }
                }
                Db.SaveChanges();
                return RedirectToAction("details", new { id = group.Id });
            }
            return View(mission);
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
