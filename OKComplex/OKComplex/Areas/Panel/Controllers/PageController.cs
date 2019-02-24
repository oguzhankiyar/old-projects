using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Models;
using System.Data;
using OKComplex.Functions;

namespace OKComplex.Areas.Panel.Controllers
{
    [PanelControl]
    public class PageController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult Index()
        {
            return View(Db.club_page.Where(x => x.IsApproval == true).OrderBy(x => x.Sort).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            club_page page = Db.club_page.Find(num);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(club_page page)
        {
            if (ModelState.IsValid)
            {
                Db.club_page.Add(page);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(page);
        }
        public ActionResult Edit(int num = 0)
        {
            club_page page = Db.club_page.Find(num);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }
        [HttpPost]
        public ActionResult Edit(club_page page)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(page).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(page);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            club_page page = Db.club_page.Find(num);
            if (page != null)
            {
                page.IsApproval = page.IsApproval == true ? false : true;
                result = page.IsApproval == true ? "Kategori başarıyla onaylandı." : "Kategorinin onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script type=\"text/javascript\">SuccessInfo('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            club_page page = Db.club_page.Find(num);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            club_page page = Db.club_page.Find(num);
            Db.club_page.Remove(page);
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
