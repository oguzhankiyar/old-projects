using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Models;
using OKComplex.Functions;

namespace OKComplex.Areas.Admin.Controllers
{
    [AdminControl]
    public class UserController : Controller
    {
        private OKDbEntities Db = new OKDbEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var user = Db.user.Include(u => u.type).OrderBy(x => x.Name);
            ViewBag.count = user.Count();
            return View(user.Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            user user = Db.user.Find(num);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult Edit(int num = 0)
        {
            user user = Db.user.Find(num);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeId = new SelectList(Db.usertype, "Id", "Name", user.TypeId);
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(user user)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(user).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeId = new SelectList(Db.usertype, "Id", "Name", user.TypeId);
            return View(user);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            user user = Db.user.Find(num);
            if (user != null)
            {
                user.IsApproval = user.IsApproval == true ? false : true;
                result = user.IsApproval == true ? "Üye başarıyla onaylandı." : "Üyenin onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script type=\"text/javascript\">SuccessInfo('" + result + "');</script>");
        }
        public ActionResult ChangeBan(int num = 0)
        {
            string result = "";
            user user = Db.user.Find(num);
            if (user != null)
            {
                user.IsBanned = user.IsBanned == true ? false : true;
                result = user.IsBanned == true ? "Üye başarıyla yasaklandı." : "Üyenin yasağı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script type=\"text/javascript\">SuccessInfo('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            user user = Db.user.Find(num);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            user user = Db.user.Find(num);
            Db.user.Remove(user);
            foreach (var topic in user.topics)
            {
                Db.topic.Remove(topic);
            }
            foreach (var comment in user.comments)
            {
                Db.comment.Remove(comment);
            }
            foreach (var post in user.posts)
            {
                Db.post.Remove(post);
            }
            foreach (var rating in user.ratings)
            {
                Db.rating.Remove(rating);
            }
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