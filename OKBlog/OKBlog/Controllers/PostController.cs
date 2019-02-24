using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKBlog.Models;

namespace OKBlog.Controllers
{
    public class PostController : Controller
    {
        private OKDbEntities db = new OKDbEntities();
        public ActionResult Details(string url)
        {
            var post = db.Posts.FirstOrDefault(x => x.ShortURL == url);
            if (Request.QueryString.AllKeys.Contains("partial") && Request.QueryString["partial"].ToString() == "yes")
                return PartialView(post);
            return View(post);
        }

    }
}
