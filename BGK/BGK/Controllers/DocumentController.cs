using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using BGK.Functions;

namespace BGK.Controllers
{
    public class DocumentController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int page)
        {
            int take = 20;
            int skip = take * (page - 1);
            var documents = Db.bgk_dokuman.Where(x => x.Onay == true).OrderByDescending(x => x.EklenmeTarihi);
            ViewBag.Title = "Dökümanlar";
            ViewBag.Message = "Henüz eklenen döküman yok :(";
            ViewBag.count = documents.Count();
            ViewBag.take = take;
            return View(documents.Skip(skip).Take(take));
        }
        public ActionResult Category(int page, int id, string seo)
        {
            var category = Db.bgk_dokuman_kategori.Find(id);
            if (category == null)
                return HttpNotFound();
            int take = 20;
            int skip = take * (page - 1);
            var documents = Db.bgk_dokuman.Where(x => x.KategoriID == id && x.Onay == true).OrderByDescending(x => x.EklenmeTarihi);
            ViewBag.Title = category.Adi + " Kategorisindeki Dökümanlar";
            ViewBag.Message = "Bu kategoriye henüz döküman eklenmemiş :(";
            ViewBag.count = documents.Count();
            ViewBag.take = take;
            return View("Index", documents.Skip(skip).Take(take));
        }
        public ActionResult Details(int id, string seo)
        {
            var document = Db.bgk_dokuman.Find(id);
            if (document == null)
                return HttpNotFound();
            return View(document);
        }
    }
}
