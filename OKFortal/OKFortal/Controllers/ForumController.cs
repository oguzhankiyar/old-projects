using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKFortal.Models;
using OKFortal.Functions;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Data;

namespace OKFortal.Controllers
{
    [SiteControl]
    public class ForumController : Controller
    {
        private OKDbEntities Db = new OKDbEntities();
        public ActionResult Index()
        {
            ViewBag.Title = "Forum";
            return View(Db.category.Where(x => x.IsApproval == true).OrderBy(x => x.Sort).ToList());
        }
        public ActionResult Category(string seo, int id)
        {
            var category = Db.category.SingleOrDefault(x => x.Id == id);
            if (category != null && category.IsApproval == true)
            {
                ViewBag.Title = category.Name;
                ViewBag.keywords = category.Name.Replace(" ", ", ");
                if (category.Seo == seo)
                    return View(category);
                else
                    return RedirectToAction("Category", new { seo = category.Seo, id = id });
            }
            else
                return HttpNotFound();
        }
        public ActionResult Forum(string seo, int id, int page)
        {
            var forum = Db.forum.SingleOrDefault(x => x.Id == id);
            if (forum != null && forum.IsApproval == true && forum.category.IsApproval == true)
            {
                var parentforum = Db.forum.SingleOrDefault(x => x.Id == forum.ParentForumId);
                if (parentforum != null ? parentforum.IsApproval == true : true)
                {
                    ViewBag.Title = forum.Name;
                    ViewBag.keywords = forum.Name.Replace(" ", ", ") + ", " + forum.category.Name.Replace(" ", ", ");
                    if (forum.Seo == seo)
                    {
                        int take = Convert.ToInt32(OK.Config("topic-paging-count"));
                        int skip = take * (page - 1);
                        var topics = OK.SortTopics(forum.topics);
                        ViewBag.topics = topics.Where(x => x.IsSticky == true).Concat(topics.Where(x => x.IsSticky == false).Skip(skip).Take(take)).ToList();
                        ViewBag.topiccount = topics.Where(x => x.IsSticky == false).Count();
                        ViewBag.stickycount = topics.Where(x => x.IsSticky == true).Count();
                        ViewBag.parentforum = parentforum;
                        forum.ViewsCount += 1;
                        Db.SaveChanges();
                        return View(forum);
                    }
                    else
                        return RedirectToAction("Forum", new { seo = forum.Seo, id = id, page = page });
                }
                else
                    return HttpNotFound();                        
            }
            else
                return HttpNotFound();
        }
        public ActionResult Topic(string seo, int id, int page)
        {
            var topic = Db.topic.SingleOrDefault(x => x.Id == id);
            if (topic != null && topic.IsApproval == true)
            {
                var parentforum = Db.forum.SingleOrDefault(x => x.Id == topic.forum.ParentForumId);
                if (topic.forum.IsSubForum == true && topic.forum.category.IsApproval == true && parentforum != null ? parentforum.IsApproval == true : true)
                {
                    ViewBag.Title = topic.Title;
                    ViewBag.keywords = topic.Title.Replace(" ", ", ") + ", " + topic.forum.Name.Replace(" ", ", ") + ", " + topic.forum.category.Name.Replace(" ", ", ");
                    if (topic.Seo == seo)
                    {
                        int take = Convert.ToInt32(OK.Config("comment-paging-count"));
                        int skip = take * (page - 1);
                        var comments = topic.comments.Where(x => x.IsApproval == true).OrderBy(x => x.CreationDate);
                        ViewBag.comments = comments.Skip(skip).Take(take).ToList();
                        ViewBag.commentcount = comments.Count();
                        ViewBag.parentforum = parentforum;
                        topic.ViewsCount += 1;
                        Db.SaveChanges();
                        return View(topic);
                    }
                    else
                        return RedirectToAction("Topic", new { seo = topic.Seo, id = id, page = page });
                }
                else
                    return HttpNotFound();
            }
            else
                return HttpNotFound();
        }
        public ActionResult New(int page)
        {
            ViewBag.keywords = "yeni konular";
            int take = Convert.ToInt32(OK.Config("topic-paging-count"));
            int skip = take * (page - 1);
            var topics = Db.topic.Where(x => x.IsApproval == true && x.forum.IsApproval == true && x.forum.category.IsApproval == true).OrderByDescending(x => x.CreationDate);
            ViewBag.count = topics.Count();
            return View(topics.Skip(skip).Take(take).ToList());
        }
        public ActionResult Popular(int page)
        {
            ViewBag.keywords = "popüler konular";
            int take = Convert.ToInt32(OK.Config("topic-paging-count"));
            int skip = take * (page - 1);
            var topics = Db.topic.Where(x => x.IsApproval == true && x.forum.IsApproval == true && x.forum.category.IsApproval == true).OrderByDescending(x => x.ViewsCount);
            ViewBag.count = topics.Count();
            return View(topics.Skip(skip).Take(take).ToList());
        }
        public ActionResult Search(int page)
        {
            string query = Request.QueryString["q"] == null ? "" : Request.QueryString["q"].ToString();
            ViewBag.keywords = query.Replace(" ", ", ");
            int take = Convert.ToInt32(OK.Config("topic-paging-count"));
            int skip = take * (page - 1);
            string sql = "";
            string[] split = OK.ConvertSeo(query).Split('-');
            for (int i = 0; i < split.Count(); i++)
            {
                sql += "Seo like '%" + split[i] + "%' or ";
            }
            sql = sql.Substring(0, sql.Length - 3);
            var topics = Db.topic.SqlQuery("Select * from topic where (" + sql + ")");
            ViewBag.count = topics.Count();
            return View(OK.SortTopics(topics).Skip(skip).Take(take).ToList());
        }
        public ActionResult GSearch()
        {
            ViewBag.Title = "Google Özel Arama";
            return View();
        }
        public JsonResult UpdateRating(int id, string score, string type)
        {
            string msg = "";
            string result = "0";
            string bars = "";
            int like = 0, dislike = 0;
            if (Session["userinfo"] == null)
            {
                msg = "Üye olmalısın";
            }
            else
            {
                int userid = (int)Session["userid"];
                if (OK.RatingScore(id, type, userid) != 0)
                {
                    result = "0";
                    msg = "Daha önce oyladın";
                }
                else
                {
                    rating rating = new rating();
                    rating.UserId = userid;
                    rating.ItemId = id;
                    rating.Score = score;
                    rating.Type = type;
                    rating.ActionDate = DateTime.Now;
                    Db.rating.Add(rating);
                    Db.SaveChanges();
                    OK.AddPost(userid, "" + id + "", (score == "+1" ? "like" : "dislike") + "-" + type, true);
                    OK.UpdateRating(userid, +5);
                    var rate = Db.rating.Where(x => x.ItemId == id && x.Type == type);
                    like = rate.Where(x => x.Score == "+1").Count();
                    dislike = rate.Where(x => x.Score == "-1").Count();
                    int xlike = like + dislike == 0 ? 50 : 100 * like / (like + dislike);
                    int xdislike = like + dislike == 0 ? 50 : 100 - (100 * like / (like + dislike));
                    if (type == "topic")
                    {
                        var topic = Db.topic.Find(id);
                        topic.Rating = like + "/" + dislike;
                    }
                    else if (type == "comment")
                    {
                        var comment = Db.comment.Find(id);
                        comment.Rating = like + "/" + dislike;
                    }
                    Db.SaveChanges();
                    result = score == "+1" ? "Bunu beğendin" : "Bunu beğenmedin";
                    bars = "<div class=\"like\" title=\"" + xlike + "%\" style=\"width: " + 180 * xlike / 100 + "px;\"></div><div class=\"dislike\" title=\"" + xdislike + "%\" style=\"width: " + 180 * xdislike / 100 + "px;\"></div>";
                    msg = "Başarıyla oyladın";
                }
            }
            return Json(new { msg = msg, result = result, bars = bars, like = like, dislike = dislike }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListingOptions()
        {
            return PartialView();
        }
        public ActionResult ListingOptionsSave()
        {
            string query = "?sort=" + Request.Form["sort"] + "&order=" + Request.Form["order"] + "&day=" + Request.Form["day"];
            return Content("<script type=\"text/javascript\">$.OK.SuccessModal('Seçenekler başarıyla işlendi. Forumlar listeleniyor..'); setTimeout(function (){ window.location.href='" + query + "'; }, 2000);</script>");
        }
        public ActionResult CreateTopic(int id)
        {
            if (Session["userinfo"] != null)
            {
                var topic = new topic() { ForumId = id, WriterId = (int)Session["userid"] };
                return PartialView(topic);
            }
            else
            {
                return Content("<script type=\"text/javascript\">$.OK.ErrorModal('Konu ekleyebilmeniz için üye girişi yapmanız gerekiyor..');</script>");
            }
        }
        public ActionResult CreateTopicSave(topic model)
        {
            model.ImageFile = null;
            model.Seo = OK.ConvertSeo(model.Title);
            model.Rating = "0/0";
            model.CreationDate = DateTime.Now;
            model.ModifyDate = DateTime.Now;
            model.IsSticky = false;
            model.IsClosed = false;
            model.IsApproval = false;
            model.ViewsCount = 0;
            Db.topic.Add(model);
            Db.SaveChanges();
            OK.AddPost((int)model.WriterId, model.ForumId + "-" + model.Id, "create-topic", false);
            return Content("<script type=\"text/javascript\">$.OK.SuccessModal('Konunuz başarıyla kaydedildi.. Onaylandıktan sonra yayınlanacak..');</script>");
        }
        public ActionResult EditTopic(int id)
        {
            var topic = Db.topic.Find(id);
            return PartialView(topic);
        }
        public ActionResult EditTopicSave(topic model)
        {
            Db.Entry(model).State = EntityState.Modified;
            model.Seo = OK.ConvertSeo(model.Title);
            model.ModifyDate = DateTime.Now;
            model.IsApproval = false;
            Db.SaveChanges();
            return Content("<script type=\"text/javascript\">$.OK.SuccessModal('Konunuz başarıyla düzenlendi.. Onaylandıktan sonra yayınlanacak..');</script>");
        }
        public ActionResult CreateComment(int id, string quote)
        {
            if (Session["userinfo"] != null)
            {
                var comment = new comment() { TopicId = id, WriterId = (int)Session["userid"], QuoteId = quote };
                return PartialView(comment);
            }
            else
            {
                return Content("<script type=\"text/javascript\">$.OK.ErrorModal('Cevap ekleyebilmeniz için üye girişi yapmanız gerekiyor..');</script>");
            }
        }
        public ActionResult CreateCommentSave(comment model)
        {
            model.Rating = "0/0";
            model.CreationDate = DateTime.Now;
            model.ModifyDate = DateTime.Now;
            model.IsApproval = false;
            Db.comment.Add(model);
            Db.SaveChanges();
            OK.AddPost((int)model.WriterId, model.TopicId + "-" + model.Id, "create-comment", false);
            return Content("<script type=\"text/javascript\">$.OK.SuccessModal('Cevabınız başarıyla kaydedildi.. Onaylandıktan sonra yayınlanacak..');</script>");
        }
        public ActionResult EditComment(int id)
        {
            var comment = Db.comment.Find(id);
            return PartialView(comment);
        }
        public ActionResult EditCommentSave(comment model)
        {
            Db.Entry(model).State = EntityState.Modified;
            model.ModifyDate = DateTime.Now;
            model.IsApproval = false;
            Db.SaveChanges();
            return Content("<script type=\"text/javascript\">$.OK.SuccessModal('Cevabınız başarıyla düzenlendi.. Onaylandıktan sonra yayınlanacak..');</script>");
        }
        public ActionResult GoTopic(int id)
        {
            var topic = Db.topic.Find(id);
            if (topic != null)
            {
                return Redirect(Url.Action("Topic", new { seo = topic.Seo, id = topic.Id, page = 1 })); // + "#t" + topic.Id
            }
            else
                return HttpNotFound();
        }
        public ActionResult GoComment(int id)
        {
            var comment = Db.comment.Find(id);
            if (comment != null)
            {
                var topic = comment.topic;
                return Redirect(Url.Action("Topic", new { seo = topic.Seo, id = topic.Id, page = OK.FindCommentPage(comment) }) + "#c" + comment.Id);
            }
            else
                return HttpNotFound();
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}