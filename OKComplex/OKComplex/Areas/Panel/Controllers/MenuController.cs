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
    public class MenuController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult Index()
        {
            return View(Db.club_menulink.OrderBy(x => x.Sort).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            club_menulink menulink = Db.club_menulink.Find(num);
            if (menulink == null)
            {
                return HttpNotFound();
            }
            return View(menulink);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(club_menulink menulink)
        {
            if (ModelState.IsValid)
            {
                Db.club_menulink.Add(menulink);
                Db.SaveChanges();
                return RedirectToAction("index");
            }

            return View(menulink);
        }
        public ActionResult Edit(int num = 0)
        {
            club_menulink menulink = Db.club_menulink.Find(num);
            if (menulink == null)
            {
                return HttpNotFound();
            }
            return View(menulink);
        }
        [HttpPost]
        public ActionResult Edit(club_menulink menulink)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(menulink).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(menulink);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            club_menulink menulink = Db.club_menulink.Find(num);
            if (menulink != null)
            {
                menulink.IsApproval = menulink.IsApproval == true ? false : true;
                result = menulink.IsApproval == true ? "Link başarıyla onaylandı." : "Linkin onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script type=\"text/javascript\">SuccessInfo('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            club_menulink menulink = Db.club_menulink.Find(num);
            if (menulink == null)
            {
                return HttpNotFound();
            }
            return View(menulink);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            club_menulink menulink = Db.club_menulink.Find(num);
            Db.club_menulink.Remove(menulink);
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
