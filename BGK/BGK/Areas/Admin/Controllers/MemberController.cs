using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using BGK.Functions;
using System.Web.Security;

namespace BGK.Areas.Admin.Controllers
{
    [Controls("MemberManagement")]
    public class MemberController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var member = Db.bgk_uye;
            ViewBag.count = member.Count();
            ViewBag.take = 10;
            return View(member.OrderBy(x => x.AdSoyad).Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_uye bgk_uye = Db.bgk_uye.Find(num);
            if (bgk_uye == null)
                return HttpNotFound();
            return View(bgk_uye);
        }
        public ActionResult Create()
        {
            ViewBag.Yetki = new SelectList(Db.bgk_yetki, "Kod", "Adi");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_uye bgk_uye)
        {
            if (ModelState.IsValid)
            {
                string password = BGKFunction.CreateCode(10);
                bool issend = BGKFunction.IsSendEmail(bgk_uye.Email, "BGK Üyeliğiniz Oluşturuldu!", "Merhaba " + bgk_uye.AdSoyad + "<br /><br />" + "Bilgi Güvenliği Kulübü üyeliğiniz oluşturulmuştur. Aşağıdaki bilgiler ile giriş yapabilirsiniz.<br /><b>Email: </b>" + bgk_uye.Email + "<br /><b>Şife: </b>" + password + "</b><br /><a href=\"" + Url.Action("Index", "Home", new { area = "" }) + "\">Siteye giriş için tıklayın.</a><br /><br />Bilgi Güvenliği Kulübü Yönetimi");
                if (Db.bgk_uye.SingleOrDefault(x => x.Email == bgk_uye.Email) != null)
                {
                    ModelState.AddModelError("Email", "Bu email adresi kullanılıyor :(");
                }
                else if (!issend)
                {
                    ModelState.AddModelError("", "Şifre gönderilemedi. Lütfen tekrar deneyin.");
                }
                else
                {
                    bgk_uye.Sifre = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5");
                    bgk_uye.Puan = 0;
                    bgk_uye.CezaPuani = 0;
                    bgk_uye.KayitTarihi = DateTime.Now;
                    bgk_uye.SonGiris = DateTime.Now;
                    Db.bgk_uye.Add(bgk_uye);
                    Db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Yetki = new SelectList(Db.bgk_yetki, "Kod", "Adi");
            return View(bgk_uye);
        }
        public ActionResult CreatePassword(int num = 0)
        {
            bgk_uye bgk_uye = Db.bgk_uye.Find(num);
            if (bgk_uye == null)
                return HttpNotFound();
            return PartialView(bgk_uye);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePassword(bgk_uye model)
        {
            var member = Db.bgk_uye.Find(model.Id);
            string password = BGKFunction.CreateCode(10);
            bool issend = BGKFunction.IsSendEmail(member.Email, "BGK Üyeliğiniz", "Merhaba " + member.AdSoyad + "<br /><br />" + "Bilgi Güvenliği Kulübü üyeliğinizin şifresi değiştirilmiştir. Aşağıdaki bilgiler ile giriş yapabilirsiniz.<br /><b>Email: </b>" + member.Email + "<br /><b>Şife: </b>" + password + "</b><br /><a href=\"" + Url.Action("Index", "Home", new { area = "" }) + "\">Siteye giriş için tıklayın.</a><br /><br />Bilgi Güvenliği Kulübü Yönetimi");
            if (issend)
            {
                member.Sifre = password;
                Db.SaveChanges();
                return Content("<script>$.BGK.SuccessModal('Şifre başarıyla değiştirildi ve ilgili üyenin email adresine postalandı.');</script>");
            }
            return Content("<font color=\"red\">Şifre gönderilemedi. Lütfen tekrar deneyin.</font>");
        
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_uye bgk_uye = Db.bgk_uye.Find(num);
            if (bgk_uye == null)
                return HttpNotFound();
            ViewBag.Yetki = new SelectList(Db.bgk_yetki, "Kod", "Adi", bgk_uye.Yetki);
            return View(bgk_uye);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_uye bgk_uye)
        {
            if (ModelState.IsValid)
            {
                if (Db.bgk_uye.SingleOrDefault(x => x.Email == bgk_uye.Email && x.Id != bgk_uye.Id) != null)
                {
                    ModelState.AddModelError("Email", "Bu email adresi kullanılıyor :(");
                }
                else
                {
                    Db.Entry(bgk_uye).State = EntityState.Modified;
                    Db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.Yetki = new SelectList(Db.bgk_yetki, "Kod", "Adi", bgk_uye.Yetki);
            return View(bgk_uye);
        }
        public ActionResult SendMessage(int num)
        {
            var member = Db.bgk_uye.Find(num);
            if (member == null && num != 0)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            bgk_mesaj message = new bgk_mesaj() { AliciID = num };
            return PartialView(message);
        }
        [HttpPost]
        public ActionResult SendMessage(bgk_mesaj message)
        {
            if (string.IsNullOrEmpty(message.Mesaj))
                return Content("<font color=\"red\">Mesaj alanını boş bıraktınız!</font>");
            if (message.AliciID == 0)
            {
                foreach (var member in Db.bgk_uye.Where(x => x.Onay == true && x.Id != Convert.ToInt32(BGKFunction.GetConfig("memberID"))).ToList())
                {
                    message.Mesaj += "<br /><br /><i><b>Not:</b> Bu mesaj kulüp yönetimi tarafından gönderilmiştir.</i>";
                    message.GonderenID = Convert.ToInt32(BGKFunction.GetConfig("adminID"));
                    message.AliciID = member.Id;
                    message.Okundu = false;
                    message.Tip = 2;
                    message.YazimTarihi = DateTime.Now;
                    Db.bgk_mesaj.Add(message);
                    Db.SaveChanges();
                }
            }
            else
            {
                message.Mesaj += "<br /><br /><i><b>Not:</b> Bu mesaj kulüp yönetimi tarafından gönderilmiştir.</i>";
                message.GonderenID = Convert.ToInt32(BGKFunction.GetConfig("adminID"));
                message.Okundu = false;
                message.Tip = 2;
                message.YazimTarihi = DateTime.Now;
                Db.bgk_mesaj.Add(message);
                Db.SaveChanges();
            }
            return Content("<script>$.BGK.SuccessModal('Mesajınız başarıyla gönderilmiştir.');</script>");
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            bgk_yazi post = Db.bgk_yazi.Find(num);
            if (post != null)
            {
                result = post.Onay ? "Yazının onayı başarıyla kaldırıldı." : "Yazı başarıyla onaylandı.";
                post.Onay = !post.Onay;
                Db.SaveChanges();
            }
            return Content("<script>$.BGK.SuccessModal('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_uye bgk_uye = Db.bgk_uye.Find(num);
            if (bgk_uye == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = bgk_uye.AdSoyad, Message = "Bu üyeyi ve bu üyeye ait herşeyi silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_uye bgk_uye = Db.bgk_uye.Find(model.Id);
            BGKFunction.DeleteMember(bgk_uye);
            Db.bgk_uye.Remove(bgk_uye);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Üye başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}