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
    public class MenuController : Controller
    {
        private OKDbEntities Db = new OKDbEntities();
        public ActionResult Index()
        {
            return View(Db.menulink.OrderBy(x => x.Sort).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            menulink menulink = Db.menulink.Find(num);
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
        public ActionResult Create(menulink menulink)
        {
            if (ModelState.IsValid)
            {
                Db.menulink.Add(menulink);
                Db.SaveChanges();
                return RedirectToAction("index");
            }

            return View(menulink);
        }
        public ActionResult Edit(int num = 0)
        {
            menulink menulink = Db.menulink.Find(num);
            if (menulink == null)
            {
                return HttpNotFound();
            }
            return View(menulink);
        }
        [HttpPost]
        public ActionResult Edit(menulink menulink)
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
            menulink menulink = Db.menulink.Find(num);
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
            menulink menulink = Db.menulink.Find(num);
            if (menulink == null)
            {
                return HttpNotFound();
            }
            return View(menulink);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            menulink menulink = Db.menulink.Find(num);
            Db.menulink.Remove(menulink);
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