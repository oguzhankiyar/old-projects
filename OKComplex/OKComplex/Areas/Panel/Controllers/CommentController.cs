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
    public class CommentController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var comment = Db.club_postcomment;
            ViewBag.count = comment.Count();
            return View(comment.OrderByDescending(x => x.WritingDate).Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            club_postcomment comment = Db.club_postcomment.Find(num);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(Db.club_post, "Id", "Title");
            return View();
        }
        [HttpPost]
        public ActionResult Create(club_postcomment comment)
        {
            if (ModelState.IsValid)
            {
                comment.MemberId = (int)Session["memberid"];
                comment.WritingDate = DateTime.Now;
                Db.club_postcomment.Add(comment);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(Db.club_post, "Id", "Title");
            return View(comment);
        }
        public ActionResult Edit(int num = 0)
        {
            club_postcomment comment = Db.club_postcomment.Find(num);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(Db.club_post, "Id", "Title");
            return View(comment);
        }
        [HttpPost]
        public ActionResult Edit(club_postcomment comment)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(comment).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.PostId = new SelectList(Db.club_post, "Id", "Title");
            return View(comment);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            club_postcomment comment = Db.club_postcomment.Find(num);
            if (comment != null)
            {
                if (comment.IsApproval == true)
                {
                    comment.IsApproval = false;
                    result = "Yorum onayı başarıyla kaldırıldı.";
                }
                else
                {
                    comment.IsApproval = true;
                    result = "Yorum başarıyla onaylandı.";
                }
                Db.SaveChanges();
            }
            return Content("<script type=\"text/javascript\">SuccessInfo('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            club_postcomment comment = Db.club_postcomment.Find(num);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            club_postcomment comment = Db.club_postcomment.Find(num);
            Db.club_postcomment.Remove(comment);
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
