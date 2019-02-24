using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using System.Web.Security;
using BGK.Functions;
using System.IO;
using System.Data;

namespace BGK.Controllers
{
    public class MemberController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        [MaintenanceControl]
        [MemberControl]
        public ActionResult Index()
        {
            return View();
        }
        [MaintenanceControl]
        [MemberControl]
        public ActionResult List(int page)
        {
            int take = 10;
            int skip = take * (page - 1);
            var members = Db.bgk_uye.Where(x => x.Onay == true).OrderBy(x => x.AdSoyad).ToList();
            ViewBag.Title = "Üyeler";
            ViewBag.count = members.Count();
            ViewBag.take = take;
            return View(members.Skip(skip).Take(take));
        }
        [MaintenanceControl]
        [MemberControl]
        public ActionResult Details(int id, string seo)
        {
            var member = Db.bgk_uye.Find(id);
            if (member == null)
                return HttpNotFound();
            return View(member);
        }
        [MaintenanceControl]
        [MemberControl]
        public ActionResult Messages(int page)
        {
            int take = 20;
            int skip = take * (page - 1);
            int memberID = (int)Session["memberID"];
            var messages = Db.bgk_mesaj.Where(x => x.GonderenID == memberID || x.AliciID == memberID).ToList();
            ViewBag.Title = "Mesaj Kutum";
            ViewBag.skip = skip;
            ViewBag.take = take;
            return View(messages.ToList());
        }
        [MaintenanceControl]
        [MemberControl]
        public ActionResult Messaging(int id)
        {
            int memberID = (int)Session["memberID"];
            if (id == memberID)
                return HttpNotFound();
            ViewBag.memberID = id;
            var messages = Db.bgk_mesaj.Where(x => x.GonderenID == id && x.AliciID == memberID);
            foreach (var message in messages)
            {
                message.Okundu = true;
            }
            Db.SaveChanges();
            return View(Db.bgk_mesaj.Where(x => (x.GonderenID == memberID && x.AliciID == id) || (x.GonderenID == id && x.AliciID == memberID)).OrderByDescending(x => x.Id).ToList());
        }
        [MaintenanceControl]
        [MemberControl]
        public ActionResult GetMessage()
        {
            int lastID = Convert.ToInt32(Request.Form["lastID"]);
            int memberID = Convert.ToInt32(Request.Form["memberID"]);
            int myID = (int)Session["memberID"];
            var messages = Db.bgk_mesaj.Where(x => x.Id > lastID && x.GonderenID == memberID && x.AliciID == myID).OrderByDescending(x => x.Id);
            foreach (var message in messages)
            {
                message.Okundu = true;
            }
            Db.SaveChanges();
            if (messages.Count() == 0)
                return Json(new { lastID = -1 }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { m = messages, lastID = messages.First().Id }, JsonRequestBehavior.AllowGet);
        }
        [MaintenanceControl]
        [HttpPost]
        [MemberControl]
        public ActionResult SendMessage(bgk_mesaj message)
        {
            message.YazimTarihi = DateTime.Now;
            message.Tip = 1;
            message.Okundu = false;
            Db.bgk_mesaj.Add(message);
            Db.SaveChanges();
            TextWriter tw = new StringWriter();
            ViewContext viewContext = new ViewContext(this.ControllerContext, new RazorView(this.ControllerContext, "anything", null, false, null), new ViewDataDictionary(), new TempDataDictionary(), tw);
            var h = new HtmlHelper(viewContext, new ViewPage());
            return Content("<div class=\"message outgoing\"><span class=\"writer\">" + BGKFunction.GetMember(message.GonderenID).AdSoyad.Split(' ')[0] + "</span><span class=\"date\">" + h.ShortDateTime(message.YazimTarihi, true).ToString() + "</span><span class=\"message\">" + message.Mesaj + "</span></div>");
        }
        [MaintenanceControl]
        [MemberControl]
        public ActionResult Groups(int id, int page, string seo)
        {
            int take = 10;
            int skip = take * (page - 1);
            var posts = Db.bgk_grup_uye.Where(x => x.UyeID == id).OrderByDescending(x => x.BitisTarihi).ToList();
            ViewBag.Title = BGKFunction.GetMember(id).AdSoyad + " > Gruplar";
            ViewBag.count = posts.Count();
            ViewBag.take = take;
            ViewBag.memberID = id;
            return View(posts.Skip(skip).Take(take));
        }
        [MaintenanceControl]
        [MemberControl]
        public ActionResult Posts(int id, int page, string seo)
        {
            int take = 10;
            int skip = take * (page - 1);
            var posts = Db.bgk_yazi.Where(x => x.UyeID == id && x.Onay == true).OrderByDescending(x => x.YazimTarihi).ToList();
            ViewBag.Title = BGKFunction.GetMember(id).AdSoyad + " > Yazılar";
            ViewBag.count = posts.Count();
            ViewBag.take = take;
            ViewBag.memberID = id;
            return View(posts.Skip(skip).Take(take));
        }
        [MaintenanceControl]
        [MemberControl]
        public ActionResult Comments(int id, int page, string seo)
        {
            int take = 10;
            int skip = take * (page - 1);
            var posts = Db.bgk_yorum.Where(x => x.UyeID == id && x.bgk_yazi.Onay == true && x.Onay == true).OrderByDescending(x => x.YazilmaTarihi).ToList();
            ViewBag.Title = BGKFunction.GetMember(id).AdSoyad + " > Yorumları";
            ViewBag.count = posts.Count();
            ViewBag.take = take;
            ViewBag.memberID = id;
            return View(posts.Skip(skip).Take(take));
        }
        [MaintenanceControl]
        [MemberControl]
        public ActionResult Missions(int id, int page, string seo)
        {
            return View();
        }
        [MaintenanceControl]
        [MemberControl]
        public ActionResult Uploads(int id, int page, string seo)
        {
            int take = 10;
            int skip = take * (page - 1);
            var uploads = Db.bgk_dosya.Where(x => x.YukleyenID == id).OrderByDescending(x => x.YuklenmeTarihi).ToList();
            ViewBag.Title = BGKFunction.GetMember(id).AdSoyad + " Adlı Üyenin Yüklemeleri";
            ViewBag.count = uploads.Count();
            ViewBag.take = take;
            ViewBag.memberID = id;
            return View(uploads.Skip(skip).Take(take));
        }
        [MaintenanceControl]
        [MemberControl]
        public ActionResult LastLoginDate()
        {
            Db.bgk_uye.Find((int)Session["memberID"]).SonGiris = DateTime.Now;
            Db.SaveChanges();
            return Content("");
        }
        [MaintenanceControl]
        [HttpPost]
        [MemberControl]
        public ActionResult ShortDateTime(DateTime datetime)
        {
            TextWriter tw = new StringWriter();
            ViewContext viewContext = new ViewContext(this.ControllerContext, new RazorView(this.ControllerContext, "anything", null, false, null), new ViewDataDictionary(), new TempDataDictionary(), tw);
            var h = new HtmlHelper(viewContext, new ViewPage());
            return Content(h.ShortDateTime(datetime, true).ToString());
        }
        [MaintenanceControl]
        [HttpPost]
        [MemberControl]
        public ActionResult MemberState(int id)
        {
            TextWriter tw = new StringWriter();
            ViewContext viewContext = new ViewContext(this.ControllerContext, new RazorView(this.ControllerContext, "anything", null, false, null), new ViewDataDictionary(), new TempDataDictionary(), tw);
            var h = new HtmlHelper(viewContext, new ViewPage());
            return Content(h.GetMemberState(id).ToString());
        }
        [MaintenanceControl]
        [HttpPost]
        [MemberControl]
        public ActionResult MemberMessageCount(int id)
        {
            if (!string.IsNullOrEmpty(Request.Form["memberID"]))
            {
                int memberID = Convert.ToInt32(Request.Form["memberID"]);
                var messages = Db.bgk_mesaj.Where(x => x.GonderenID == memberID && x.AliciID == id && x.Okundu == false);
                return Content(messages.Count().ToString());
            }
            else
            {
                var messages = Db.bgk_mesaj.Where(x => x.AliciID == id && x.Okundu == false);
                return Content(messages.Count().ToString());
            }
        }
        [MaintenanceControl]
        [HttpPost]
        [MemberControl]
        public ActionResult MyNotificationCount()
        {
            return Content(BGKFunction.GetMyAccount().bgk_bildirim.Where(x => x.Okundu == false).Count().ToString());
        }
        [MaintenanceControl]
        [HttpPost]
        [MemberControl]
        public ActionResult MyNotifications()
        {
            var notifications = BGKFunction.GetMyAccount().bgk_bildirim.OrderByDescending(x => x.Tarih).ToList();
            foreach (var notification in notifications)
            {
                notification.Okundu = true;
            }
            Db.SaveChanges();
            return PartialView(notifications);
        }



        [MemberControl]
        public ActionResult ChangePassword()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
	        {
                int memberID = (int)Session["memberID"];
                var member = Db.bgk_uye.Find(memberID);
                string password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Sifre.ToString(), "md5").ToString();
                member.Sifre = password;
                Db.SaveChanges();
                Session["memberInfo"] = null;
                Session["memberID"] = 0;
                Session["memberRole"] = 0;
                HttpCookie cookie_memberID = new HttpCookie("BGK_memberID");
                HttpCookie cookie_password = new HttpCookie("BGK_password");
                cookie_memberID.Expires = DateTime.Now.AddDays(-1);
                cookie_password.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie_memberID);
                Response.Cookies.Add(cookie_password);
                return Content("<script>$.BGK.SuccessModal('Şifreniz başarılı bir şekilde değiştirildi. Yeniden giriş yapmanız gerekiyor.', function (){ window.location.href='" + Url.Action("Index", "Home") + "'; }, 1500);</script>");
            }
            return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
        }
        public ActionResult Login()
        {
            return PartialView();
        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(bgk_uye model)
        {
            if (ModelState.IsValid)
	        {
                string password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Sifre.ToString(), "md5").ToString();
                int result = 0;
                int studentID = int.TryParse(model.UserName, out result) ? Convert.ToInt32(model.UserName) : 0;
                var member = Db.bgk_uye.SingleOrDefault(x => (x.Email == model.UserName || x.OgrenciNo == studentID) && x.Sifre == password && x.Onay == true);
                if (member == null)
                    return Content("<font color=\"red\">Giriş başarısız</font>");
                else
                {
                    Session["memberInfo"] = member;
                    Session["memberID"] = member.Id;
                    Session["memberRole"] = member.Yetki;
                    if (model.RememberMe == true)
                    {
                        HttpCookie cookie_memberID = new HttpCookie("BGK_memberID");
                        cookie_memberID.Value = member.Id.ToString();
                        cookie_memberID.Expires = DateTime.Now.AddDays(365);
                        HttpCookie cookie_password = new HttpCookie("BGK_password");
                        cookie_password.Value = password;
                        cookie_password.Expires = DateTime.Now.AddDays(365);
                        Response.Cookies.Add(cookie_memberID);
                        Response.Cookies.Add(cookie_password);
                    }
                    return Content("<script>$.BGK.SuccessModal('Hesabınıza başarılı bir şekilde giriş yaptınız.', function (){ window.location.reload(); }, 1500);</script>");
                }
            }
            return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
        }*/
        [MemberControl]
        public ActionResult Logout()
        {
            Session["memberInfo"] = null;
            Session["memberID"] = 0;
            Session["memberRole"] = 0;
            HttpCookie cookie_memberID = new HttpCookie("BGK_memberID");
            HttpCookie cookie_password = new HttpCookie("BGK_password");
            cookie_memberID.Expires = DateTime.Now.AddDays(-1);
            cookie_password.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie_memberID);
            Response.Cookies.Add(cookie_password);
            return Content("<script>$.BGK.SuccessModal('Hesabınızdan başarılı bir şekilde çıkış yaptınız.', function (){ window.location.href='" + Url.Action("Index", "Home") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
