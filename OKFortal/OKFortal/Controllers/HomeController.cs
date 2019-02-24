using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKFortal.Models;
using OKFortal.Functions;

namespace OKFortal.Controllers
{
    public class HomeController : Controller
    {
        private OKDbEntities Db = new OKDbEntities();
        [SiteControl]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Forum");
        }
        [SiteControl]
        public ActionResult About()
        {
            return View();
        }
        [SiteControl]
        public ActionResult OKFortal()
        {
            return View();
        }
        public ActionResult Membership()
        {
                if (Session["userinfo"] != null)
                {
                    int id = (int)Session["userid"];
                    var user = Db.user.Single(x => x.Id == id);
                    user.LastLoginDate = DateTime.Now;
                    Db.SaveChanges();
                    return PartialView(user);
                }
                else
                {
                    return PartialView();
                }
        }
        public ActionResult Menu()
        {
            if (OK.Config("site-on/off") != "0" || Session["role"].ToString() == "10")
                return PartialView(Db.menulink.Where(x => x.IsApproval == true).OrderBy(x => x.Sort).ToList());
            else
                return null;
        }
        public ActionResult Notices()
        {
            if (OK.Config("site-on/off") != "0" || Session["role"].ToString() == "10")
                return PartialView(Db.notice.Where(x => x.IsApproval == true).OrderBy(x => x.Sort).ToList());
            else
                return null;
        }
        public ActionResult NoticeDetail(int id)
        {
            return PartialView(Db.notice.Find(id));
        }
        public ActionResult SiteMap()
        {
            List<SitemapItem> veriler = new List<SitemapItem>();
            veriler.Add(new SitemapItem() { loc = Url.Action("index", "home", null, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            veriler.Add(new SitemapItem() { loc = Url.Action("about", "home", null, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            veriler.Add(new SitemapItem() { loc = Url.Action("new", "forum", new { page = 1 }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            veriler.Add(new SitemapItem() { loc = Url.Action("popular", "forum", new { page = 1 }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            veriler.Add(new SitemapItem() { loc = Url.Action("search", "forum", null, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            veriler.Add(new SitemapItem() { loc = Url.Action("users", "account", new { type = "all", page = 1 }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            foreach (var category in Db.category.Where(x => x.IsApproval == true))
            {
                veriler.Add(new SitemapItem() { loc = Url.Action("category", "forum", new { id = category.Id, seo = category.Seo }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            }
            foreach (var forum in Db.forum.Where(x => x.IsApproval == true && x.category.IsApproval == true).ToList())
            {
                var parentforum = Db.forum.SingleOrDefault(x => x.Id == forum.ParentForumId);
                if (parentforum != null ? parentforum.IsApproval == true : true)
                    veriler.Add(new SitemapItem() { loc = Url.Action("forum", "forum", new { id = forum.Id, seo = forum.Seo }, "http"), lastmod = forum.topics.Count() == 0 ? DateTime.Now : (DateTime)forum.topics.OrderByDescending(x => x.ModifyDate).First().ModifyDate, changefreq = "daily", priority = "1" });
            }
            foreach (var topic in Db.topic.Where(x => x.IsApproval == true && x.forum.IsApproval == true && x.forum.category.IsApproval == true))
            {
                veriler.Add(new SitemapItem() { loc = Url.Action("topic", "forum", new { id = topic.Id, seo = topic.Seo }, "http"), lastmod = (DateTime)topic.ModifyDate, changefreq = "daily", priority = "1" });
            }
            foreach (var user in Db.user)
            {
                veriler.Add(new SitemapItem() { loc = Url.Action("info", "account", new { username = user.UserName }, "http"), lastmod = (DateTime)user.RegistrationDate, changefreq = "daily", priority = "1" });
            }
            Sitemap sitemap = new Sitemap(veriler);
            return Content(sitemap.Result(), "text/xml");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
