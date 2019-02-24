using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Models;
using System.Web.Security;
using OKComplex.Functions;

namespace OKComplex.Controllers
{
    public class MemberController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult LoginForm()
        {
            if (Session["memberinfo"] != null)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }
        [HttpPost]
        public ActionResult Login(club_member model)
        {
            string result;
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password.ToString(), "md5").ToString();
            var member = Db.club_member.SingleOrDefault(x => x.Email == model.Email && x.Password == password);
            if (member == null)
            {
                result = "<font color=red>Böyle bir hesap bulunamadı..</font>";
            }
            else
            {
                Session["memberinfo"] = member;
                Session["memberid"] = member.Id;
                Session["memberrole"] = member.Role;
                result = "<font color=green>Hesabınıza giriş yapılıyor..</font><script type=\"text/javascript\">setTimeout(function () { window.location.href='" + Url.Action("Index", "Home") + "'; }, 2000);</script>";
            }
            return Content(result);
        }
        public ActionResult Logout()
        {
            Session["memberinfo"] = null;
            Session["memberid"] = 0;
            Session["memberrole"] = "0";
            return RedirectToAction("Index", "Home");
        }
        [ClubSiteControl]
        public ActionResult Info(int id)
        {
            var member = Db.club_member.Find(id);
            if (member == null || member.IsApproval == false)
                return HttpNotFound();
            else
                return View(member);
        }
    }
}
