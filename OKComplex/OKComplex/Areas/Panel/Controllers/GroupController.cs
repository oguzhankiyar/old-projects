using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Models;
using OKComplex.Functions;
using System.Data;

namespace OKComplex.Areas.Panel.Controllers
{
    [PanelControl]
    public class GroupController : Controller
    {
        private InfosecEntities Db = new InfosecEntities();
        public ActionResult Index()
        {
            return View(Db.club_group.Where(x => x.IsSubGroup == false).OrderBy(x => x.Sort).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            club_group group = Db.club_group.Find(num);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(club_group group)
        {
            if (ModelState.IsValid)
            {
                group.Seo = OK.ConvertSeo(group.Name);
                Db.club_group.Add(group);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }
        public ActionResult Edit(int num = 0)
        {
            club_group group = Db.club_group.Find(num);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }
        [HttpPost]
        public ActionResult Edit(club_group group)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(group).State = EntityState.Modified;
                group.Seo = OK.ConvertSeo(group.Name);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }
        public ActionResult Delete(int num = 0)
        {
            club_group group = Db.club_group.Find(num);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int num)
        {
            club_group group = Db.club_group.Find(num);
            foreach (var post in group.club_posts.ToList())
            {
                Db.club_post.Remove(post);
            }
            foreach (var member in group.club_members.ToList())
            {
                Db.club_groupmember.Remove(member);
            }
            Db.club_group.Remove(group);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult MemberList(int num)
        {
            var group = Db.club_group.Find(num);
            if (group == null)
                return HttpNotFound();
            else
                return View(group);
        }
        public ActionResult AddMember(int num)
        {
            club_group group = Db.club_group.Find(num);
            if (group == null)
                return HttpNotFound();
            else
            {
                var groupmember = new club_groupmember() { club_group = group };
                return View(groupmember);
            }
        }
        [HttpPost]
        public ActionResult AddMember(club_groupmember groupmember)
        {
            groupmember.RegistrationDate = DateTime.Now;
            Db.club_groupmember.Add(groupmember);
            Db.SaveChanges();
            return RedirectToAction("Details");
        }
        public ActionResult EditMember(int num)
        {
            club_groupmember groupmember = Db.club_groupmember.Find(num);
            if (groupmember == null)
                return HttpNotFound();
            else
                return View(groupmember);
        }
        [HttpPost]
        public ActionResult EditMember(club_groupmember groupmember)
        {
            Db.Entry(groupmember).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("Details");
        }
        public ActionResult DeleteMember(int num)
        {
            club_groupmember groupmember = Db.club_groupmember.Find(num);
            if (groupmember == null)
                return HttpNotFound();
            else
                return View(groupmember);
        }
        [HttpPost, ActionName("DeleteMember")]
        public ActionResult DeleteMember(club_groupmember groupmember)
        {
            Db.club_groupmember.Remove(groupmember);
            Db.SaveChanges();
            return RedirectToAction("memberlist");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
