using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Models;
using System.Data;
using OKComplex.Functions;

namespace OKComplex.Areas.Panel.Controllers
{
    [PanelControl]
    public class MemberController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var member = Db.club_member.OrderBy(x => x.Name);
            ViewBag.count = member.Count();
            return View(member.Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            club_member member = Db.club_member.Find(num);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(club_member member)
        {
            if (ModelState.IsValid)
            {
                Db.club_member.Add(member);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }
        public ActionResult Edit(int num = 0)
        {
            club_member member = Db.club_member.Find(num);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }
        [HttpPost]
        public ActionResult Edit(club_member member)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(member).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            club_member member = Db.club_member.Find(num);
            if (member != null)
            {
                member.IsApproval = member.IsApproval == true ? false : true;
                result = member.IsApproval == true ? "Üye başarıyla onaylandı." : "Üyenin onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script type=\"text/javascript\">SuccessInfo('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            club_member member = Db.club_member.Find(num);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            club_member member = Db.club_member.Find(num);
            foreach (var post in member.club_posts)
            {
                Db.club_post.Remove(post);
            }
            foreach (var comment in member.club_comments)
            {
                Db.club_postcomment.Remove(comment);
            }
            Db.club_member.Remove(member);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
