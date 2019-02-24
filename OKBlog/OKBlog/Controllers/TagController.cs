using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKBlog.Models;

namespace OKBlog.Controllers
{
    public class TagController : Controller
    {
        OKDbEntities db = new OKDbEntities();

        public ActionResult Index()
        {
            var tags = db.Tags.Where(x => x.Approval);
            if (Request.QueryString.AllKeys.Contains("partial") && Request.QueryString["partial"].ToString() == "yes")
                return PartialView(tags);
            return View(tags);
        }

    }
}
