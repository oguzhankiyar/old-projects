using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialObisis.Models;

namespace SocialObisis.Controllers
{
    public class LessonController : Controller
    {
        public ActionResult Index(int id = 0, int page = 0)
        {
            Obisis.Ogrenci ogr = (Obisis.Ogrenci)Session["student"];
            if (id == 0)
                return View(ogr.Dersler);
            return View(ogr.Dersler.Where(x => x.Donem.No == id && x.Donem.OgretimYiliKodu == page));
        }
        public ActionResult Details(string id)
        {
            Obisis.Ogrenci ogr = (Obisis.Ogrenci)Session["student"];
            var lesson = ogr.Dersler.SingleOrDefault(x => x.Kod == id);
            if (lesson == null)
                return HttpNotFound();
            return View(lesson);
        }
        public ActionResult Gano()
        {
            Obisis.Ogrenci ogr = (Obisis.Ogrenci)Session["student"];
            ViewBag.GANO = ogr.GANO;
            return View(ogr.Donemler.First().Dersler);
        }
        [HttpPost]
        public JsonResult Deneme()
        {
            Obisis.Ogrenci ogr = (Obisis.Ogrenci)Session["student"];
            IEnumerable<Obisis.Ders> dersler = ogr.Donemler.First().Dersler;
            List<int> ortalamalar = new List<int>();
            List<string> harfler = new List<string>();
            foreach (Obisis.Ders item in dersler)
            {
                if (!string.IsNullOrEmpty(Request.Form[item.Kod + "_Vize1"]))
                    item.Vize1 = Convert.ToInt32(Request.Form[item.Kod + "_Vize1"]);
                if (!string.IsNullOrEmpty(Request.Form[item.Kod + "_Vize2"]))
                    item.Vize2 = Convert.ToInt32(Request.Form[item.Kod + "_Vize2"]);
                if (!string.IsNullOrEmpty(Request.Form[item.Kod + "_Vize3"]))
                    item.Vize3 = Convert.ToInt32(Request.Form[item.Kod + "_Vize3"]);
                if (!string.IsNullOrEmpty(Request.Form[item.Kod + "_Final"]))
                    item.Final = Convert.ToInt32(Request.Form[item.Kod + "_Final"]);
                if (!string.IsNullOrEmpty(Request.Form[item.Kod + "_Butunleme"]))
                    item.Butunleme = Convert.ToInt32(Request.Form[item.Kod + "_Butunleme"]);
                if (Request.Form[item.Kod + "_OrtalamayaEtki"] == "1")
                    item.OrtalamayaEtki = true;
                else
                    item.OrtalamayaEtki = false;
                ortalamalar.Add(Obisis.Ortalama(item));
                harfler.Add(Obisis.CalculateMark(item));
            }
            Session["student"] = new Obisis.Ogrenci(Session["ogrenciNo"].ToString(), Session["sifre"].ToString());
            return Json(new { gano = Obisis.CalculateGANO(ogr, dersler), ort = ortalamalar, harf = harfler }, JsonRequestBehavior.AllowGet);
            //return Content(Obisis.CalculateGANO(ogr, dersler).ToString());
        }
        [HttpPost]
        public ActionResult CalculateGano()
        {
            Obisis.Ogrenci ogr = (Obisis.Ogrenci)Session["student"];
            IEnumerable<Obisis.Ders> dersler = ogr.Donemler.First().Dersler;
            foreach (Obisis.Ders item in dersler)
            {
                if (!string.IsNullOrEmpty(Request.Form[item.Kod + "_Vize1"]))
                    item.Vize1 = Convert.ToInt32(Request.Form[item.Kod + "_Vize1"]);
                if (!string.IsNullOrEmpty(Request.Form[item.Kod + "_Vize2"]))
                    item.Vize2 = Convert.ToInt32(Request.Form[item.Kod + "_Vize2"]);
                if (!string.IsNullOrEmpty(Request.Form[item.Kod + "_Vize3"]))
                    item.Vize3 = Convert.ToInt32(Request.Form[item.Kod + "_Vize3"]);
                if (!string.IsNullOrEmpty(Request.Form[item.Kod + "_Final"]))
                    item.Final = Convert.ToInt32(Request.Form[item.Kod + "_Final"]);
                if (!string.IsNullOrEmpty(Request.Form[item.Kod + "_Butunleme"]))
                    item.Butunleme = Convert.ToInt32(Request.Form[item.Kod + "_Butunleme"]);
                if (Request.Form[item.Kod + "_OrtalamayaEtki"] == "1")
                    item.OrtalamayaEtki = true;
                else
                    item.OrtalamayaEtki = false;
            }
            Session["student"] = new Obisis.Ogrenci(Session["ogrenciNo"].ToString(), Session["sifre"].ToString());
            return Content(Obisis.CalculateGANO(ogr, dersler).ToString());
            //ViewBag.GANO = Obisis.CalculateGANO(ogr, dersler).ToString();
            //return View("Gano", dersler);
        }
    }
}
