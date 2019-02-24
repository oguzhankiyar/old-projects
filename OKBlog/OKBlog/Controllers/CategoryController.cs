using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKBlog.Models;

namespace OKBlog.Controllers
{
    public class CategoryController : Controller
    {
        private OKDbEntities db = new OKDbEntities();

        public ActionResult Index()
        {
            var categories = db.Categories.Where(x => x.Approval);
            if (Request.QueryString.AllKeys.Contains("partial") && Request.QueryString["partial"].ToString() == "yes")
                return PartialView(categories);
            return View(categories);
        }

        public ActionResult Details(string url)
        {
            var category = db.Categories.FirstOrDefault(x => x.ShortURL == url);
            if (Request.QueryString.AllKeys.Contains("partial") && Request.QueryString["partial"].ToString() == "yes")
                return PartialView(category);
            return View(category);
        }
    }
}
