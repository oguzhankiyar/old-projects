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
    public class GroupController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult List()
        {
            ViewBag.Title = "Komiteler";
            return View(Db.club_group.Where(x => x.IsSubGroup == false).OrderBy(x => x.Sort).ToList());
        }
        public ActionResult Detail(string seo)
        {
            var group = Db.club_group.SingleOrDefault(x => x.Seo == seo);
            if (group != null)
            {
                ViewBag.Title = group.Name;
                return View(group);
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult Members(string seo)
        {
            var group = Db.club_group.SingleOrDefault(x => x.Seo == seo);
            if (group != null)
            {
                ViewBag.Title = group.Name + " Üyeleri";
                return View(group);
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult SubGroups(string seo)
        {
            var group = Db.club_group.SingleOrDefault(x => x.Seo == seo);
            if (group != null)
            {
                ViewBag.Title = group.Name + " Alt Grupları";
                return View(group);
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult Posts(string seo)
        {
            var group = Db.club_group.SingleOrDefault(x => x.Seo == seo);
            if (group != null)
            {
                ViewBag.Title = group.Name + " Gönderileri";
                return View(group);
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult PostControl()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ChangePostApproval()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            var post = Db.club_post.Find(id);
            if (post != null)
            {
                int memberid = (int)Session["memberid"];
                var control = Db.club_groupmember.SingleOrDefault(x => x.MemberId == memberid && x.GroupId == post.GroupId && x.Role == 1);
                if (control != null)
                {
                    post.IsApproval = !post.IsApproval;
                    Db.SaveChanges();
                }
            }
            return Content("");
        }
    }
}
