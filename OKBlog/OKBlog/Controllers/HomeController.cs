using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKBlog.Models;

namespace OKBlog.Controllers
{
    public class HomeController : Controller
    {
        private OKDbEntities db = new OKDbEntities();

        public ActionResult Index()
        {
            var posts = db.Posts.Where(x => x.Approval).OrderByDescending(x => x.CreatedAt).ToList();
            if (Request.QueryString.AllKeys.Contains("partial") && Request.QueryString["partial"].ToString() == "yes")
                return PartialView(posts);
            return View(posts);
        }

        public ActionResult About()
        {
            if (Request.QueryString.AllKeys.Contains("partial") && Request.QueryString["partial"].ToString() == "yes")
                return PartialView();
            return View();
        }

    }
}
