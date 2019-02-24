using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Models;
using OKComplex.Functions;

namespace OKComplex.Areas.Admin.Controllers
{
    [AdminControl]
    public class CommentController : Controller
    {
        private OKDbEntities Db = new OKDbEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var comment = Db.comment;
            ViewBag.count = comment.Count();
            return View(comment.Include(c => c.topic).Include(c => c.user).OrderByDescending(x => x.ModifyDate).Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            comment comment = Db.comment.Find(num);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        public ActionResult Create()
        {
            ViewBag.TopicId = new SelectList(Db.topic, "Id", "Title");
            return View();
        }
        [HttpPost]
        public ActionResult Create(comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.WriterId = (int)Session["userid"];
                comment.Rating = "0/0";
                comment.CreationDate = DateTime.Now;
                comment.ModifyDate = DateTime.Now;
                Db.comment.Add(comment);
                Db.SaveChanges();
                OK.UpdateRating((int)Session["userid"], +15);
                OK.AddPost((int)comment.WriterId, comment.TopicId.ToString() + "-" + comment.Id.ToString(), "create-comment", (bool)comment.IsApproval);
                return RedirectToAction("Index");
            }
            ViewBag.TopicId = new SelectList(Db.topic, "Id", "Title", comment.TopicId);
            return View(comment);
        }
        public ActionResult Edit(int num = 0)
        {
            comment comment = Db.comment.Find(num);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TopicId = new SelectList(Db.topic, "Id", "Title", comment.TopicId);
            return View(comment);
        }
        [HttpPost]
        public ActionResult Edit(comment comment)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(comment).State = EntityState.Modified;
                var control = Db.comment.Find(comment.Id);
                if (control.TopicId != comment.TopicId)
                {
                    Db.post.SingleOrDefault(x => x.ItemId == control.TopicId.ToString() + "-" + control.Id.ToString() && x.Type == "create-topic").ItemId = comment.TopicId + "-" + comment.Id;
                }
                if (comment.ChangeModifyDate)
                {
                    comment.ModifyDate = DateTime.Now;
                }
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.TopicId = new SelectList(Db.topic, "Id", "Title", comment.TopicId);
            return View(comment);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            comment comment = Db.comment.Find(num);
            if (comment != null)
            {
                if (comment.IsApproval == true)
                {
                    comment.IsApproval = false;
                    result = "Cevabın onayı başarıyla kaldırıldı.";
                    OK.UpdateRating((int)comment.WriterId, -15);
                    OK.ChangePostApproval((int)comment.WriterId, comment.TopicId.ToString() + "-" + comment.Id.ToString(), "create-comment", false);
                    foreach (var likepost in Db.post.Where(x => x.ItemId == comment.Id.ToString() && (x.Type == "like-comment" || x.Type == "dislike-comment")))
                    {
                        likepost.IsApproval = false;
                    }
                }
                else
                {
                    comment.IsApproval = true;
                    result = "Cevap başarıyla onaylandı.";
                    OK.UpdateRating((int)comment.WriterId, +15);
                    OK.ChangePostApproval((int)comment.WriterId, comment.TopicId.ToString() + "-" + comment.Id.ToString(), "create-comment", true);
                    foreach (var likepost in Db.post.Where(x => x.ItemId == comment.Id.ToString() && (x.Type == "like-comment" || x.Type == "dislike-comment")))
                    {
                        likepost.IsApproval = true;
                    }
                }
                Db.SaveChanges();
            }
            return Content("<script type=\"text/javascript\">SuccessInfo('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            comment comment = Db.comment.Find(num);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            comment comment = Db.comment.Find(num);
            foreach (var rating in Db.rating.Where(x => x.ItemId == comment.Id && x.Type == "comment"))
            {
                Db.rating.Remove(rating);
            }
            foreach (var likepost in Db.post.Where(x => x.ItemId == comment.Id.ToString() && (x.Type == "like-comment" || x.Type == "dislike-comment")))
            {
                Db.post.Remove(likepost);
            }
            Db.comment.Remove(comment);
            Db.SaveChanges();
            if (comment.IsApproval == true)
                OK.UpdateRating((int)comment.WriterId, -15);
            OK.DeletePost((int)comment.WriterId, comment.TopicId.ToString() + "-" + comment.Id.ToString(), "create-comment");
            return RedirectToAction("index");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}