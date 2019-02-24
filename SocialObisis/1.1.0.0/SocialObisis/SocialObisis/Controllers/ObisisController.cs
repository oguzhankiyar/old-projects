using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialObisis.Models;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Text;

namespace SocialObisis.Controllers
{
    public class ObisisController : Controller
    {
        /*
         * 1020216186
         * 12306
         */
        public ActionResult Index()
        {
            return Redirect(Url.Action("Login"));
        }
        public ActionResult Login()
        {
            if (Session["sifre"] == null)
                return View();
            return RedirectToAction("Info");
        }
        [HttpPost]
        public ActionResult DoLogin()
        {
            try
            {
                Session["ogrenciNo"] = Request.Form["OgrenciNo"];
                Session["sifre"] = Request.Form["Sifre"];
                Obisis.Ogrenci ogr = new Obisis.Ogrenci(Session["ogrenciNo"].ToString(), Session["sifre"].ToString());
                Session["student"] = ogr;
                Obisis.AddLoginLog(DateTime.Now + " - " + ogr.AdSoyad + " - " + ogr.Fakulte + " " + ogr.Bolum +  " " + ogr.Sinif);
            }
            catch (Exception)
            {
                Session["ogrenciNo"] = Session["sifre"] = Session["student"] = null;
                return Content("error");
            }
            return Content("success");
        }
        public ActionResult Info()
        {
            Obisis.Ogrenci ogr = (Obisis.Ogrenci)Session["student"];
            return View(ogr);
        }
        public ActionResult Notices()
        {
            return View((new Obisis()).Duyurular);
        }
        public ActionResult Logout()
        {
            Session["ogrenciNo"] = null;
            Session["sifre"] = null;
            return RedirectToAction("Index");
        }
    }
}