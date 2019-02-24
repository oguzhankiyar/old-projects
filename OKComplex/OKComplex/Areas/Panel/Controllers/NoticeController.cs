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
    public class NoticeController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult Index()
        {
            return View(Db.club_notice.OrderBy(x => x.Sort).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            club_notice notice = Db.club_notice.Find(num);
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
        public ActionResult Create(club_notice notice)
        {
            if (ModelState.IsValid)
            {
                notice.WritingDate = DateTime.Now;
                Db.club_notice.Add(notice);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(notice);
        }
        public ActionResult Edit(int num = 0)
        {
            club_notice notice = Db.club_notice.Find(num);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(club_notice notice)
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
            club_notice notice = Db.club_notice.Find(num);
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
            club_notice notice = Db.club_notice.Find(num);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            club_notice notice = Db.club_notice.Find(num);
            Db.club_notice.Remove(notice);
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
