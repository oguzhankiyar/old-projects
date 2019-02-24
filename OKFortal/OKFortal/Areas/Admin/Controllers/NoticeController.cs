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
    public class NoticeController : Controller
    {
        private OKDbEntities Db = new OKDbEntities();
        public ActionResult Index()
        {
            return View(Db.notice.OrderBy(x => x.Sort).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            notice notice = Db.notice.Find(num);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(notice notice)
        {
            if (ModelState.IsValid)
            {
                notice.CreationDate = DateTime.Now;
                Db.notice.Add(notice);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(notice);
        }
        public ActionResult Edit(int num = 0)
        {
            notice notice = Db.notice.Find(num);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(notice notice)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(notice).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(notice);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            notice notice = Db.notice.Find(num);
            if (notice != null)
            {
                notice.IsApproval = notice.IsApproval == true ? false : true;
                result = notice.IsApproval == true ? "Duyuru başarıyla onaylandı." : "Duyurunun onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script type=\"text/javascript\">SuccessInfo('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            notice notice = Db.notice.Find(num);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            notice notice = Db.notice.Find(num);
            Db.notice.Remove(notice);
            Db.SaveChanges();
            return RedirectToAction("index");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}