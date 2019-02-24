using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Models;
using OKComplex.Functions;

namespace OKComplex.Controllers
{
    public class HomeController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        [ClubSiteControl]
        public ActionResult Index()
        {
            ViewBag.Title = "Anasayfa";
            return View();
        }
        public ActionResult Membership()
        {
            return PartialView();
        }
        [ClubSiteControl]
        public ActionResult Menu()
        {
            return PartialView(Db.club_menulink.Where(x => x.IsApproval == true && x.Type == 1).OrderBy(x => x.Sort).ToList());
        }
        public ActionResult BottomMenu()
        {
            return PartialView("Menu", Db.club_menulink.Where(x => x.IsApproval == true && x.Type == 2).OrderBy(x => x.Sort).ToList());
        }
        [ClubSiteControl]
        public ActionResult Notices()
        {
            return PartialView(Db.club_notice.Where(x => x.IsApproval == true).OrderBy(x => x.Sort));
        }
        [ClubSiteControl]
        public ActionResult MinimalNotices()
        {
            return PartialView(Db.club_notice.Where(x => x.IsApproval == true).OrderBy(x => x.Sort));
        }
        [ClubSiteControl]
        public ActionResult NoticeDetail(int id, string seo)
        {
            var notice = Db.club_notice.SingleOrDefault(x => x.Id == id);
            if (notice == null)
                return HttpNotFound();
            else if (seo != notice.Seo)
                return RedirectToAction("NoticeDetail", "Home", new { id = id, seo = seo });
            else
            {
                ViewBag.Title = notice.Title;
                return View(notice);
            }
        }
        [ClubSiteControl]
        public ActionResult Search()
        {
            return PartialView();
        }
        [ClubSiteControl]
        public ActionResult Categories()
        {
            return PartialView(Db.club_category.Where(x => x.IsApproval == true).ToList());
        }
        [ClubSiteControl]
        public ActionResult RecentPosts()
        {
            return PartialView(Db.club_post.Where(x => x.IsApproval == true && x.club_category.IsApproval == true).OrderByDescending(x => x.WritingDate).Take(15).ToList());
        }
        [ClubSiteControl]
        public ActionResult PopularPosts()
        {
            return PartialView(Db.club_post.Where(x => x.IsApproval == true && x.club_category.IsApproval == true).OrderByDescending(x => x.ViewsCount).Take(15).ToList());
        }
        [ClubSiteControl]
        public ActionResult Facebook()
        {
            return PartialView();
        }
    }
}