using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Models;
using OKComplex.Functions;
using System.Data;

namespace OKComplex.Areas.Panel.Controllers
{
    [PanelControl]
    public class CategoryController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult Index()
        {
            return View(Db.club_category.OrderBy(x => x.Sort).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            club_category category = Db.club_category.Find(num);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(club_category category)
        {
            if (ModelState.IsValid)
            {
                category.Seo = OK.ConvertSeo(category.Name);
                Db.club_category.Add(category);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(category);
        }
        public ActionResult Edit(int num = 0)
        {
            club_category category = Db.club_category.Find(num);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost]
        public ActionResult Edit(club_category category)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(category).State = EntityState.Modified;
                category.Seo = OK.ConvertSeo(category.Name);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(category);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            club_category category = Db.club_category.Find(num);
            if (category != null)
            {
                category.IsApproval = category.IsApproval == true ? false : true;
                result = category.IsApproval == true ? "Kategori başarıyla onaylandı." : "Kategorinin onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script type=\"text/javascript\">SuccessInfo('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            club_category category = Db.club_category.Find(num);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            club_category category = Db.club_category.Find(num);
            Db.club_category.Remove(category);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
