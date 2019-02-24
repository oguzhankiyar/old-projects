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
    [Controls("GroupManagement")]
    public class GroupController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var group = Db.bgk_grup.OrderByDescending(x => x.Id);
            ViewBag.count = group.Count();
            ViewBag.take = take;
            return View(group.Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_grup bgk_grup = Db.bgk_grup.Find(num);
            if (bgk_grup == null)
                return HttpNotFound();
            return View(bgk_grup);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_grup bgk_grup)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_grup.Add(bgk_grup);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bgk_grup);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_grup bgk_grup = Db.bgk_grup.Find(num);
            if (bgk_grup == null)
                return HttpNotFound();
            return View(bgk_grup);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_grup bgk_grup)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(bgk_grup).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bgk_grup);
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_grup bgk_grup = Db.bgk_grup.Find(num);
            if (bgk_grup == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = bgk_grup.Adi, Message = "Bu grubu ve tüm üyelikleri silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_grup bgk_grup = Db.bgk_grup.Find(model);
            BGKFunction.DeleteGroup(bgk_grup);
            Db.bgk_grup.Remove(bgk_grup);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Grup başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        public ActionResult SendMessage(int num)
        {
            var group = Db.bgk_grup.Find(num);
            if (group == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            bgk_mesaj message = new bgk_mesaj() { GonderenID = group.Id };
            return PartialView(message);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public ActionResult MemberDetails(int num = 0)
        {
            bgk_grup_uye bgk_grup_uye = Db.bgk_grup_uye.Find(num);
            if (bgk_grup_uye == null)
                return HttpNotFound();
            return View(bgk_grup_uye);
        }
        public ActionResult AddMember(int num)
        {
            var group = Db.bgk_grup.Find(num);
            if (group == null)
                return HttpNotFound();
            bgk_grup_uye member = new bgk_grup_uye() { GrupID = group.Id };
            ViewBag.groupName = group.Adi;
            ViewBag.UyeID = new SelectList(Db.bgk_uye, "Id", "AdSoyad");
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember(bgk_grup_uye bgk_grup_uye)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_grup_uye.Add(bgk_grup_uye);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UyeID = new SelectList(Db.bgk_uye, "Id", "AdSoyad");
            return View(bgk_grup_uye);
        }
        public ActionResult EditMember(int num)
        {
            var member = Db.bgk_grup_uye.Find(num);
            if (member == null)
                return HttpNotFound();
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMember(bgk_grup_uye bgk_grup_uye)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_grup_uye.Add(bgk_grup_uye);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bgk_grup_uye);
        }
        public ActionResult DeleteMember(int num = 0)
        {
            bgk_grup_uye bgk_grup_uye = Db.bgk_grup_uye.Find(num);
            if (bgk_grup_uye == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return View("DeleteActions", new delete_action() { Id = num, Title = "Grup Üyeliğini Sil", Message = "Bu üyeliği silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMember(delete_action model)
        {
            bgk_grup_uye bgk_grup_uye = Db.bgk_grup_uye.Find(model.Id);
            Db.bgk_grup_uye.Remove(bgk_grup_uye);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Grup başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("details", new { num = bgk_grup_uye.GrupID }) + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}