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
    public class TopicController : Controller
    {
        private OKDbEntities Db = new OKDbEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var topic = Db.topic;
            ViewBag.count = topic.Count();
            return View(topic.Include(t => t.forum).Include(t => t.user).OrderByDescending(x => x.ModifyDate).Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            topic topic = Db.topic.Find(num);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }
        public ActionResult Create()
        {
            ViewBag.ForumId = new SelectList(Db.forum, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(topic topic)
        {
            if (ModelState.IsValid)
            {
                int userid = (int)Session["userid"];
                topic.WriterId = userid;
                topic.Seo = OK.ConvertSeo(topic.Title);
                topic.CreationDate = DateTime.Now;
                topic.ModifyDate = DateTime.Now;
                topic.Rating = "0/0";
                topic.ViewsCount = 0;
                Db.topic.Add(topic);
                Db.SaveChanges();
                OK.AddPost(userid, topic.ForumId.ToString() + "-" + topic.Id.ToString(), "create-topic", (bool)topic.IsApproval);
                OK.UpdateRating(userid, +25);
                return RedirectToAction("index");
            }
            ViewBag.ForumId = new SelectList(Db.forum, "Id", "Name", topic.ForumId);
            return View(topic);
        }
        public ActionResult Edit(int num = 0)
        {
            topic topic = Db.topic.Find(num);
            if (topic == null)
            {
                return HttpNotFound();
            }
            ViewBag.ForumId = new SelectList(Db.forum, "Id", "Name", topic.ForumId);
            return View(topic);
        }
        [HttpPost]
        public ActionResult Edit(topic topic)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(topic).State = EntityState.Modified;
                topic.Seo = OK.ConvertSeo(topic.Title);
                var control = Db.topic.Find(topic.Id);
                if (control.ForumId != topic.ForumId)
                {
                    Db.post.SingleOrDefault(x => x.ItemId == control.ForumId.ToString() + "-" + control.Id.ToString() && x.Type == "create-topic").ItemId = topic.ForumId + "-" + topic.Id;
                }
                if (topic.ChangeModifyDate)
                {
                    topic.ModifyDate = DateTime.Now;
                }
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.ForumId = new SelectList(Db.forum, "Id", "Name", topic.ForumId);
            return View(topic);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            topic topic = Db.topic.Find(num);
            if (topic != null)
            {
                if (topic.IsApproval == true)
                {
                    topic.IsApproval = false;
                    result = "Konunun onayı başarıyla kaldırıldı.";
                    OK.UpdateRating((int)topic.WriterId, -25);
                    OK.ChangePostApproval((int)topic.WriterId, topic.ForumId + "-" + topic.Id, "create-topic", false);
                    foreach (var comment in topic.comments.ToList())
                    {
                        foreach (var commentpost in Db.post.Where(x => x.ItemId == num + "-" + comment.Id && x.Type == "create-comment").ToList())
                        {
                            commentpost.IsApproval = false;
                        }
                        foreach (var likecommentpost in Db.post.Where(x => x.ItemId == "" + comment.Id + "" && (x.Type == "like-comment" || x.Type == "dislike-comment")).ToList())
                        {
                            likecommentpost.IsApproval = false;
                        }
                    }
                    foreach (var likepost in Db.post.Where(x => x.ItemId == "" + topic.Id + "" && (x.Type == "like-topic" || x.Type == "dislike-topic")).ToList())
                    {
                        likepost.IsApproval = false;
                    }
                }
                else
                {
                    topic.IsApproval = true;
                    result = "Konu başarıyla onaylandı.";
                    OK.UpdateRating((int)topic.WriterId, +25);
                    OK.ChangePostApproval((int)topic.WriterId, topic.ForumId.ToString() + "-" + topic.Id.ToString(), "create-topic", true);
                    foreach (var comment in topic.comments)
                    {
                        foreach (var commentpost in Db.post.Where(x => x.ItemId == num + "-" + comment.Id && x.Type == "create-comment").ToList())
                        {
                            commentpost.IsApproval = true;
                        }
                        foreach (var likecommentpost in Db.post.Where(x => x.ItemId == "" + comment.Id + "" && (x.Type == "like-comment" || x.Type == "dislike-comment")).ToList())
                        {
                            likecommentpost.IsApproval = true;
                        }
                    }
                    foreach (var likepost in Db.post.Where(x => x.ItemId == "" + topic.Id + "" && (x.Type == "like-topic" || x.Type == "dislike-topic")))
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
            topic topic = Db.topic.Find(num);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            topic topic = Db.topic.Find(num);
            foreach (var rating in Db.rating.Where(x => x.ItemId == topic.Id && x.Type == "topic"))
            {
                Db.rating.Remove(rating);
            }
            foreach (var comment in topic.comments)
            {
                Db.comment.Remove(comment);
                foreach (var commentpost in Db.post.Where(x => x.ItemId == num.ToString() + "-" + comment.Id.ToString() && x.Type == "create-comment"))
                {
                    Db.post.Remove(commentpost);
                }
                foreach (var likepost in Db.post.Where(x => x.ItemId == comment.Id.ToString() && (x.Type == "like-comment" || x.Type == "dislike-comment")))
                {
                    Db.post.Remove(likepost);
                }
            }
            foreach (var likepost in Db.post.Where(x => x.ItemId == topic.Id.ToString() && (x.Type == "like-topic" || x.Type == "dislike-topic")))
            {
                Db.post.Remove(likepost);
            }
            Db.topic.Remove(topic);
            Db.SaveChanges();
            if (topic.IsApproval == true)
                OK.UpdateRating((int)topic.WriterId, -25);
            OK.DeletePost((int)topic.WriterId, topic.ForumId.ToString() + "-" + topic.Id.ToString(), "create-topic");
            return RedirectToAction("index");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}