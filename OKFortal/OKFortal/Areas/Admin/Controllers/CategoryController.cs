using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKFortal.Models;
using OKFortal.Functions;

namespace OKFortal.Areas.Admin.Controllers
{
    [AdminControl]
    public class CategoryController : Controller
    {
        private OKDbEntities Db = new OKDbEntities();
        public ActionResult Index()
        {
            return View(Db.category.OrderBy(x => x.Sort).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            category category = Db.category.Find(num);
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
        public ActionResult Create(category category)
        {
            if (ModelState.IsValid)
            {
                category.Seo = OK.ConvertSeo(category.Name);
                Db.category.Add(category);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(category);
        }
        public ActionResult Edit(int num = 0)
        {
            category category = Db.category.Find(num);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost]
        public ActionResult Edit(category category)
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
            category category = Db.category.Find(num);
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
            category category = Db.category.Find(num);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            category category = Db.category.Find(num);
            Db.category.Remove(category);
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