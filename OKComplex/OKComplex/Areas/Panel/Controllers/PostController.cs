using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Models;
using OKComplex.Functions;

namespace OKComplex.Areas.Panel.Controllers
{
    [PanelControl]
    public class PostController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var post = Db.club_post;
            ViewBag.count = post.Count();
            return View(post.OrderByDescending(x => x.ModifyDate).Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            club_post post = Db.club_post.Find(num);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(Db.club_category, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(club_post post)
        {
            if (ModelState.IsValid)
            {
                int userid = (int)Session["memberid"];
                post.MemberId = userid;
                post.Seo = OK.ConvertSeo(post.Title);
                post.WritingDate = DateTime.Now;
                post.ModifyDate = DateTime.Now;
                Db.club_post.Add(post);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.CategoryId = new SelectList(Db.club_category, "Id", "Name");
            return View(post);
        }
        public ActionResult Edit(int num = 0)
        {
            club_post post = Db.club_post.Find(num);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(Db.club_category, "Id", "Name");
            return View(post);
        }
        [HttpPost]
        public ActionResult Edit(club_post post)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(post).State = EntityState.Modified;
                post.Seo = OK.ConvertSeo(post.Title);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.CategoryId = new SelectList(Db.club_category, "Id", "Name");
            return View(post);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            club_post post = Db.club_post.Find(num);
            if (post != null)
            {
                if (post.IsApproval == true)
                {
                    post.IsApproval = false;
                    result = "Yazının onayı başarıyla kaldırıldı.";
                }
                else
                {
                    post.IsApproval = true;
                    result = "Yazı başarıyla onaylandı.";
                }
                Db.SaveChanges();
            }
            return Content("<script type=\"text/javascript\">SuccessInfo('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            club_post post = Db.club_post.Find(num);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            club_post post = Db.club_post.Find(num);
            foreach (var comment in post.club_comments.ToList())
            {
                Db.club_postcomment.Remove(comment);
            }
            Db.club_post.Remove(post);
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