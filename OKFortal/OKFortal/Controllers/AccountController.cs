using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OKFortal.Functions;
using OKFortal.Models;
using System.Data.Entity.Validation;
using System.Data;
using System.IO;

namespace OKFortal.Controllers
{
    public class AccountController : Controller
    {
        private OKDbEntities Db = new OKDbEntities();
        [SiteControl]
        public ActionResult UserInfo(int id)
        {
            return PartialView(Db.user.Find(id));
        }
        public ActionResult LoginForm()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Login(user model)
        {
            string result;
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password.ToString(), "md5").ToString();
            var user = Db.user.SingleOrDefault(x => x.UserName == model.UserName && x.Password == password);
            if (user == null)
	        {
		        result = "Böyle bir hesap bulunamadı..";
	        }
            else
            {
                if (user.IsBanned == true)
                {
                    result = "<script type=\"text/javascript\">$.OK.ErrorModal('Hesabınızın sitemize girişi yasaklanmıştır..');</script>";
                }
                else if (user.IsApproval == false)
                {
                    result = "<script type=\"text/javascript\">$.OK.ErrorModal('Hesabınız henüz onaylanmamış, daha sonra tekrar deneyiniz..');</script>";
                }
                else
                {
                    Session["userinfo"] = user;
                    Session["userid"] = user.Id;
                    Session["role"] = 10; //user.TypeId.ToString();
                    if (model.RememberMe == true)
                    {
                        HttpCookie cookie_username = new HttpCookie("OK_userid");
                        cookie_username.Value = user.Id.ToString();
                        cookie_username.Expires = DateTime.Now.AddDays(365);
                        HttpCookie cookie_password = new HttpCookie("OK_password");
                        cookie_password.Value = password;
                        cookie_password.Expires = DateTime.Now.AddDays(365);
                        Response.Cookies.Add(cookie_username);
                        Response.Cookies.Add(cookie_password);
                    }
                    user.LoginCount += 1;
                    Db.SaveChanges();
                    result = "<script type=\"text/javascript\">$.OK.SuccessModal('Hesabınıza giriş yapılıyor..');setTimeout(function () { window.location.href='" + Url.Action("Index", "Home") + "'; }, 2000);</script>";
                }
            }
            return Content(result);
        }
        public ActionResult Logout()
        {
            int id = (int)Session["userid"];
            Db.user.Single(x => x.Id == id).LastLoginDate = DateTime.Now.AddMinutes(-6);
            Db.SaveChanges();
            Session["userinfo"] = null;
            Session["userid"] = 0;
            Session["userseo"] = "";
            Session["role"] = "";
            HttpCookie cookie_userid = new HttpCookie("OK_userid");
            HttpCookie cookie_password = new HttpCookie("OK_password");
            cookie_userid.Expires = DateTime.Now.AddDays(-1);
            cookie_password.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie_userid);
            Response.Cookies.Add(cookie_password);
            return Content("<script type=\"text/javascript\">$.OK.SuccessModal('Hesabınızdan çıkış yapılıyor..');setTimeout(function () { window.location.href = '/'; }, 2000);</script>");
        }
        [SiteControl]
        public ActionResult ChangePassword()
        {
            var user = new ChangePasswordModel() { Id = (int)Session["userid"] };
            return PartialView(user);
        }
        [HttpPost]
        [SiteControl]
        public ActionResult ChangePasswordSave(user model)
        {
            string result;
            string oldpassword = FormsAuthentication.HashPasswordForStoringInConfigFile(model.OldPassword, "md5").ToString();
            var user = Db.user.Find(model.Id);
            if (user.Password != oldpassword)
            {
                result = "<font color=red>Şuanki şifrenizi yanlış girdiniz.. Tekrar deneyin..</font>";
            }
            else
            {
                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.NewPassword ,"md5");
                Db.SaveChanges();
                result = "<script type=\"text/javascript\">$.OK.SuccessModal('Şifreniz başarıyla değiştirildi.. Hesabınıza yeniden giriş yapmanız gerekiyor..'); setTimeout(function (){ $.post('" + Url.Action("Logout", "Account") + "'); $.OK.OpenNewPage('" + Url.Action("Index", "Home") + "'); }, 2000);</script>";
            }
            return Content(result);
        }
        [SiteControl]
        public ActionResult MyPassword(int step)
        {
            return PartialView();
        }
        [HttpPost]
        [SiteControl]
        public ActionResult MyPasswordStep1(user model)
        {
            string result;
            var user = Db.user.SingleOrDefault(x => x.UserName == model.UserName && x.Email == model.Email);
            if (user == null)
            {
                result = "<font color=red>Böyle bir hesap bulunamadı.. Tekrar deneyin..</font>";
            }
            else
            {
                result = "<script type=\"text/javascript\">$.OK.FormModal('" + Url.Action("MyPassword", "Account", new { step = 2, userid = user.Id }) + "', 'Devam', 'Vazgeç')</script>";
                Session["MyPassword_ConfirmationCode"] = OK.CreateConfirmationCode("MP");
                Db.SaveChanges();
                OK.SendEmail(model.Email, "Şifre Değiştirme Onay Kodunuz - " + OK.Config("site-name"), "<p>Merhaba " + model.Name + ",</p><p>Şifre değiştirme işlemine aşağıdaki kodu kullanarak devam edebilirsiniz..</p><p>Onay Kodunuz: <b>" + Session["MyPassword_ConfirmationCode"] + "</b></p>");
            }
            return Content(result);
        }
        [HttpPost]
        [SiteControl]
        public ActionResult MyPasswordStep2(user model)
        {
            string result;
            var user = Db.user.SingleOrDefault(x => x.Id == model.Id);
            if (model.ConfirmationCode == Session["MyPassword_ConfirmationCode"].ToString())
            {
                result = "<script type=\"text/javascript\">$.OK.FormModal('" + Url.Action("MyPassword", "Account", new { step = 3, userid = user.Id }) + "', 'Devam', 'Vazgeç');</script>";
            }
            else
            {
                result = "<font color=red>Onay kodunu yanlış girdiniz.. Tekrar deneyin..</font>";
            }
            return Content(result);
        }
        [HttpPost]
        [SiteControl]
        public ActionResult MyPasswordStep3(user model)
        {
            var user = Db.user.SingleOrDefault(x => x.Id == model.Id);
            user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.NewPassword.ToString(), "md5").ToString();
            Db.SaveChanges();
            return Content("<script type=\"text/javascript\">$.OK.SuccessModal('Şifreniz başarıyla değiştirildi.. Giriş yapabilirsiniz..'); setTimeout(function (){ $.OK.FormModal('" + Url.Action("LoginForm", "Account") + "', 'Giriş', 'Vazgeç'); }, 2000);</script>");
        }
        [SiteControl]
        public ActionResult Register(int step)
        {
            return PartialView();
        }
        [HttpPost]
        [SiteControl]
        public ActionResult RegisterStep1(user model)
        {
            string result;
            var users = Db.user;
            if (users.Where(x => x.UserName == model.UserName).Count() != 0)
            {
                result = "<font color=red>Bu kullanıcı adı başkası tarafından kullanılıyor..</font>";
            }
            else if (users.Where(x => x.Email == model.Email).Count() != 0)
            {
                result = "<font color=red>Bu email başkası tarafından kullanılıyor..</font>";
            }
            else
            {
                Session["Register_Email"] = model.Email;
                Session["Register_ConfirmationCode"] = OK.CreateConfirmationCode("UR");
                OK.SendEmail(model.Email, "Üyelik Onay Kodunuz - " + OK.Config("site-name"), "<p>Merhaba</p><p>Üyelik kaydı işlemine aşağıdaki kodu kullanarak devam edebilirsiniz..</p><p>Onay Kodunuz: <b>" + Session["Register_ConfirmationCode"] + "</b></p>");
                result = "<script type=\"text/javascript\">$.OK.FormModal('" + Url.Action("Register", "Account", new { step = 3 }) + "', 'Devam', 'Vazgeç')</script>";
            }
            return Content(result);
        }
        [HttpPost]
        [SiteControl]
        public ActionResult RegisterStep2(user model)
        {
            string result;
            if (model.ConfirmationCode == Session["Register_ConfirmationCode"].ToString())
            {
                result = "<script type=\"text/javascript\">$.OK.FormModal('" + Url.Action("Register", "Account", new { step = 4 }) + "', 'Devam', 'Vazgeç')</script>";
            }
            else
            {
                result = "<font color=red>Onay kodunu yanlış girdiniz.. Tekrar deneyin..</font>";
            }
            return Content(result);
        }
        [HttpPost]
        [SiteControl]
        public ActionResult RegisterStep3(user model)
        {
            string result;
            var control = Db.user.Where(x => x.UserName == model.UserName);
            if (control.Count() == 0)
            {
                model.Email = Session["Register_Email"].ToString();
                model.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password.ToString(), "md5").ToString();
                model.ImageFile = Url.Content("~/Uploads/Images/user/noimage.jpg");
                model.LoginCount = 0;
                model.LastLoginDate = DateTime.Now;
                model.RegistrationDate = DateTime.Now;
                model.Rating = 250;
                model.TypeId = 1;
                model.IsApproval = true;
                model.IsBanned = false;
                model.DisplayType = 1;
                Db.user.Add(model);
                Db.SaveChanges();
                OK.SendEmail(model.Email, "Üyelik İşleminiz Tamamlandı - " + OK.Config("site-name"), "<p>Merhaba " + model.Name + ",</p><p>Üyelik işleminiz tamamlandı.. Aramıza katıldığın için teşekkür ederiz..</p>");
                result = "<script type=\"text/javascript\">$.OK.SuccessModal('Kaydınız başarıyla tamamlandı.. Giriş yapabilirsiniz..'); setTimeout(function (){ $.OK.FormModal('" + Url.Action("LoginForm", "Account") + "', 'Giriş', 'Vazgeç'); }, 2000);</script>";
            }
            else
            {
                result = "<font color=red>Bu Kullanıcı Adı başkası tarafından kullanılıyor..</font>";
            }
            return Content(result);
        }
        [SiteControl]
        public ActionResult UserMenu(string username)
        {
            var user = Db.user.SingleOrDefault(x => x.UserName == username);
            if (user != null)
            {
                ViewBag.keywords = user.UserName + ", " + user.Name;
                return PartialView(user);
            }
            else
                return HttpNotFound();
        }
        [SiteControl]
        public ActionResult Settings()
        {
            if (Session["userinfo"] != null)
                return View(Db.user.Find((int)Session["userid"]));
            else
                return HttpNotFound();
        }
        [SiteControl]
        [HttpPost]
        public ActionResult SettingsSave(user user)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(user).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("settings", "account");
            }
            return View(user);
        }
        [SiteControl]
        public ActionResult ChangePicture()
        {
            return PartialView();
        }
        [SiteControl]
        [HttpPost]
        public ActionResult ChangePictureSave(HttpPostedFileBase ImageUpload)
        {
            string result = "";
            if (ImageUpload != null && ImageUpload.ContentLength > 0) 
            {
                var fileName = Path.GetFileName(OK.ConvertSeo("" + Session["userid"] + "-" + DateTime.Now + "") + ".jpg");
                var path = Server.MapPath("~/Uploads/Images/User");
                var path2 = Path.Combine(path, fileName);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                ImageUpload.SaveAs(path2);
                var user = Db.user.Find((int)Session["userid"]);
                if (System.IO.File.Exists(Url.Content("~" + user.ImageFile)))
                    System.IO.File.Delete(Url.Content("~" + user.ImageFile));
                user.ImageFile = Url.Content("~/Uploads/Images/User") + "/" + fileName;
                Db.SaveChanges();
                result = "<font color=green>Profil resminiz başarıyla değiştirildi..</font><script type=\"text/javascript\">window.reload();</script>";
            }
            return Content(result);
        }
        [SiteControl]
        public ActionResult Users(int page)
        {
            ViewBag.keywords = "üyeler";
            int take = 10;
            int skip = take * (page - 1);
            var users = Db.user.Where(x => x.IsApproval == true).OrderBy(x => x.Name);
            ViewBag.Title = "Üyeler";
            ViewBag.count = users.Count();
            return View(users.Skip(skip).Take(take).ToList());
        }
        [SiteControl]
        public ActionResult Info(string username)
        {
            var user = Db.user.SingleOrDefault(x => x.UserName == username);
            if (user != null)
                return View(user);
            else
                return HttpNotFound();
        }
        [SiteControl]
        public ActionResult MessageBox()
        {
            int id = (int)Session["userid"];
            return View(Db.message.Where(x => x.SenderId == id || x.ReceiverId == id).OrderByDescending(x => x.SendingDate).ToList());
        }
        [SiteControl]
        public ActionResult Messages()
        {
            string username = Request.Form["username"];
            var user = Db.user.Where(x => x.UserName == username);
            if (user.Count() > 0)
	        {
                int userid = (int)Session["userid"];
                int id = user.First().Id;
		        return PartialView(Db.message.Where(x => (x.SenderId == userid && x.ReceiverId == id) || (x.SenderId == id && x.ReceiverId == userid)).OrderByDescending(x => x.SendingDate).ToList());
	        }
            else
	        {
                return PartialView();
	        }
        }
        [SiteControl]
        public ActionResult Actions(string username, int page)
        {
            int take = 10;
            int skip = take * (page - 1);
            var user = Db.user.SingleOrDefault(x => x.UserName == username);
            if (user != null)
            {
                var actions = user.posts.Where(x => x.IsApproval == true).OrderByDescending(x => x.CreationDate);
                ViewBag.Title = OK.UserName(user) + "'ın Yaptığı İşlemler";
                ViewBag.count = actions.Count();
                return View(actions.Skip(skip).Take(take).ToList());
            }
            else
                return HttpNotFound();
        }
        [SiteControl]
        public ActionResult Topics(string username, int page)
        {
            int take = 10;
            int skip = take * (page - 1);
            var user = Db.user.SingleOrDefault(x => x.UserName == username);
            if (user != null)
            {
                var topics = user.topics.Where(x => x.IsApproval == true).OrderByDescending(x => x.ModifyDate);
                ViewBag.Title = OK.UserName(user) + "'ın Yazdığı Konular";
                ViewBag.count = topics.Count();
                return View(topics.Skip(skip).Take(take).ToList());
            }
            else
                return HttpNotFound();
        }
        [SiteControl]
        public ActionResult Comments(string username, int page)
        {
            int take = 10;
            int skip = take * (page - 1);
            var user = Db.user.SingleOrDefault(x => x.UserName == username);
            if (user != null)
            {
                var comments = user.comments.Where(x => x.IsApproval == true).OrderByDescending(x => x.ModifyDate);
                ViewBag.Title = OK.UserName(user) + "'ın Yazdığı Cevaplar";
                ViewBag.count = comments.Count();
                return View(comments.Skip(skip).Take(take).ToList());
            }
            else
                return HttpNotFound();
        }
        [SiteControl]
        public ActionResult Ratings(string username, int page)
        {
            int take = 10;
            int skip = take * (page - 1);
            var user = Db.user.SingleOrDefault(x => x.UserName == username);
            if (user != null)
            {
                var ratings = user.ratings.OrderByDescending(x => x.ActionDate);
                ViewBag.Title = OK.UserName(user) + "'ın Oyladığı İçerikler";
                ViewBag.count = ratings.Count();
                return View(ratings.Skip(skip).Take(take).ToList());
            }
            else
                return HttpNotFound();
        }
        [SiteControl]
        public ActionResult SendMessage(int id)
        {
            var message = new message() { SenderId = (int)Session["userid"], ReceiverId = id };
            ViewBag.receiver = OK.UserName(Db.user.Find(id));
            return PartialView(message);
        }
        [SiteControl]
        public ActionResult SendMessageSave(message model)
        {
            model.SendingDate = DateTime.Now;
            model.IsRead = false;
            Db.message.Add(model);
            Db.SaveChanges();
            return Content("<script type=\"text/javascript\">$.OK.SuccessModal('Mesajınız başarıyla gönderilmiştir..');</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
