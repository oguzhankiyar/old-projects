using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Models;
using OKComplex.Functions;

namespace OKComplex.Controllers
{
    [ClubSiteControl]
    public class PostController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult List(int page)
        {
            int take = 7;
            int skip = take * (page - 1);
            var posts = Db.club_post.Where(x => x.IsApproval == true && x.club_category.IsApproval == true).OrderByDescending(x => x.WritingDate);
            ViewBag.Title = "Yazılar";
            ViewBag.count = posts.Count();
            return View(posts.Skip(skip).Take(take).ToList());
        }
        public ActionResult Posts(int page)
        {
            int take = 7;
            int skip = take * (page - 1);
            var posts = Db.club_post.Where(x => x.IsApproval == true && x.club_category.IsApproval == true).OrderByDescending(x => x.WritingDate);
            ViewBag.Title = "Yazılar";
            ViewBag.count = posts.Count();
            return PartialView("List", posts.Skip(skip).Take(take).ToList());
        }
        public ActionResult Category(string seo, int page)
        {
            var category = Db.club_category.SingleOrDefault(x => x.Seo == seo && x.IsApproval == true);
            if (category == null)
                return HttpNotFound();
            else
            {
                int take = 10;
                int skip = take * (page - 1);
                var posts = category.club_posts.Where(x => x.IsApproval == true).OrderByDescending(x => x.WritingDate);
                ViewBag.Title = category.Name + " Kategorisine Ait Yazılar";
                ViewBag.count = posts.Count();
                ViewBag.CategoryId = category.Id;
                ViewBag.categoryname = category.Name;
                return View(posts.Skip(skip).Take(take).ToList());
            }
        }
        public ActionResult Group(int id, int page)
        {
            var group = Db.club_group.Find(id);
            int take = 10;
            int skip = take * (page - 1);
            var posts = group.club_posts.Where(x => x.IsApproval == true).OrderByDescending(x => x.WritingDate);
            ViewBag.count = posts.Count();
            return PartialView(posts.Skip(skip).Take(take).ToList());
        }
        public ActionResult Detail(int id, string seo, int page)
        {
            var post = Db.club_post.Find(id);
            int memberid = (int)Session["memberid"];
            var groupmember = post.GroupId == null || memberid == 0 ? null : Db.club_groupmember.SingleOrDefault(x => x.MemberId == memberid && x.GroupId == post.GroupId && x.Role == 1);
            if (post == null || (post.IsApproval == false && groupmember == null) || post.CategoryId != null ? (post.club_category.IsApproval == false) : post.club_group == null)
                return HttpNotFound();
            else if (post.Seo != seo)
                return RedirectToAction("Detail", "Post", new { id = id, seo = post.Seo, page = 1 });
            else
            {
                int take = 10;
                int skip = take * (page - 1);
                var comments = post.club_comments.Where(x => x.IsApproval == true).OrderByDescending(x => x.WritingDate);
                post.ViewsCount++;
                Db.SaveChanges();
                ViewBag.Title = post.Title;
                ViewBag.comments = comments.Skip(skip).Take(take).ToList();
                ViewBag.commentcount = comments.Count();
                return View(post);
            }
        }
        public ActionResult Search(int page)
        {
            string query = Request.QueryString["q"] == null ? "" : Request.QueryString["q"].Trim().ToString();
            ViewBag.keywords = query.Replace(" ", ", ");
            int take = 15;
            int skip = take * (page - 1);
            string sql = "";
            string[] split = OK.ConvertSeo(query).Split('-');
            for (int i = 0; i < split.Count(); i++)
            {
                sql += "Seo like '%" + split[i] + "%' or ";
            }
            sql = sql.Substring(0, sql.Length - 3);
            var posts = Db.club_post.SqlQuery("Select * from club_post where IsApproval = 1 and (" + sql + ")");
            ViewBag.Title = "Arama: " + query;
            ViewBag.count = posts.Count();
            return View(posts.Skip(skip).Take(take).ToList()); //kategori onay kontrolüü!!!
        }
        public ActionResult CreatePost()
        {
            if (Session["memberinfo"] == null)
                return HttpNotFound();
            else
                return View();
        }
        [HttpPost]
        public ActionResult CreatePost(club_post model)
        {
            if (ModelState.IsValid)
            {
                model.Seo = OK.ConvertSeo(model.Title);
                model.MemberId = (int)Session["memberid"];
                model.WritingDate = DateTime.Now;
                model.ModifyDate = DateTime.Now;
                var groupmember = Db.club_groupmember.SingleOrDefault(x => x.GroupId == model.GroupId && x.MemberId == model.MemberId);
                model.IsApproval = groupmember == null ? false : (groupmember.Role == 1 ? true : false);
                Db.club_post.Add(model);
                Db.SaveChanges();
                ViewBag.message = model.IsApproval == true ? "<font color=green>Yazı başarıyla yayınlandı.</font><script typr=\"text/javascript\">setTimeout(function (){ $.OK.OpenNewPage('" + Url.Action("Index", "Home") + "'); }, 2000);</script>" : "<font color=green>Yazı başarıyla eklendi. Yöneticiler tarafından onaylandıktan sonra yayınlanacak.</font><script typr=\"text/javascript\">setTimeout(function (){ $.OK.OpenNewPage('" + Url.Action("Index", "Home") + "'); }, 2000);</script>";
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateCommentSave(club_postcomment model)
        {
            string result;
            if (ModelState.IsValid)
            {
                if (model.Comment == null || (Session["memberinfo"] == null && model.WriterName == null))
                {
                    result = "<font color=red>Boş bıraktığınız alan var..</font>";
                }
                else
                {
                    model.WritingDate = DateTime.Now;
                    if (Session["memberinfo"] == null)
                    {
                        model.IsApproval = false;
                        result = "<font color=green>Yorumunuz başarıyla kaydedildi.. Onaylandıktan sonra yayınlanacak..</font><script typr=\"text/javascript\">setTimeout(function (){ ToggleCommentBar(); }, 2000);</script>";
                    }
                    else
                    {
                        model.IsApproval = true;
                        result = "<font color=green>Yorumunuz başarıyla yayınlandı..</font><script typr=\"text/javascript\">setTimeout(function (){ window.location.reload(); }, 2000);</script>";
                    }
                    Db.club_postcomment.Add(model);
                    Db.SaveChanges();
                }
            }
            else
            {
                result = "<font color=red>Boş alan bıraktınız. Tekrar deneyin.</font>";
            }
            return Content(result);
        }
    }
}
