using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKPort.Models;

namespace OKPort.Controllers
{
    public class PageController : Controller
    {
        private OKDbEntities Db = new OKDbEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(string pageURL)
        {
            var page = Db.Pages.SingleOrDefault(x => x.ShortURL == pageURL && x.IsApproval);
            if (page == null)
                return HttpNotFound();
            return View(page);
        }
        public ActionResult Section(string pageURL, string sectionURL)
        {
            var page = Db.Pages.SingleOrDefault(x => x.ShortURL == pageURL && x.IsApproval);
            if (page == null)
                return HttpNotFound();
            var section = page.Sections.SingleOrDefault(x => x.ShortURL == sectionURL && x.IsApproval);
            if (section == null)
                return HttpNotFound();
            return View(section);
        }
        public ActionResult Category(string pageURL, string sectionURL, string categoryURL)
        {
            var page = Db.Pages.SingleOrDefault(x => x.ShortURL == pageURL && x.IsApproval);
            if (page == null)
                return HttpNotFound();
            var section = page.Sections.SingleOrDefault(x => x.ShortURL == sectionURL && x.IsApproval);
            if (section == null)
                return HttpNotFound();
            var category = section.Categories.SingleOrDefault(x => x.ShortURL == categoryURL && x.IsApproval);
            if (category == null)
                return HttpNotFound();
            return View(category);
        }
        public ActionResult Post(string pageURL, string sectionURL, string categoryURL, string postURL)
        {
            var page = Db.Pages.SingleOrDefault(x => x.ShortURL == pageURL && x.IsApproval);
            if (page == null)
                return HttpNotFound();
            var section = page.Sections.SingleOrDefault(x => x.ShortURL == sectionURL && x.IsApproval);
            if (section == null)
                return HttpNotFound();
            var category = section.Categories.SingleOrDefault(x => x.ShortURL == categoryURL && x.IsApproval);
            if (category == null)
                return HttpNotFound();
            var post = category.Posts.SingleOrDefault(x => x.ShortURL == postURL && x.IsApproval);
            if (post == null)
                return HttpNotFound();
            return View(post);
        }
    }
}