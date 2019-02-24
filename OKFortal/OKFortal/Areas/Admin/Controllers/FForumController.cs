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
    public class FForumController : Controller
    {
        private OKDbEntities Db = new OKDbEntities();
        public ActionResult Index()
        {
            return View(Db.category.OrderBy(x => x.Sort).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            forum forum = Db.forum.Find(num);
            if (forum == null)
            {
                return HttpNotFound();
            }
            return View(forum);
        }
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(Db.category, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(forum forum)
        {
            if (ModelState.IsValid)
            {
                forum.Seo = OK.ConvertSeo(forum.Name);
                Db.forum.Add(forum);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(Db.category, "Id", "Name", forum.CategoryId);
            return View(forum);
        }
        public ActionResult Edit(int num = 0)
        {
            forum forum = Db.forum.Find(num);
            if (forum == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(Db.category, "Id", "Name", forum.CategoryId);
            return View(forum);
        }
        [HttpPost]
        public ActionResult Edit(forum forum)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(forum).State = EntityState.Modified;
                forum.Seo = OK.ConvertSeo(forum.Name);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(Db.category, "Id", "Name", forum.CategoryId);
            return View(forum);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            forum forum = Db.forum.Find(num);
            if (forum != null)
            {
                forum.IsApproval = forum.IsApproval == true ? false : true;
                result = forum.IsApproval == true ? "Forum başarıyla onaylandı." : "Forumun onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script type=\"text/javascript\">SuccessInfo('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            forum forum = Db.forum.Find(num);
            if (forum == null)
            {
                return HttpNotFound();
            }
            return View(forum);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            forum forum = Db.forum.Find(num);
            Db.forum.Remove(forum);
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