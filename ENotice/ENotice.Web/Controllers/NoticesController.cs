using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ENotice.Web.Models;

namespace ENotice.Web.Controllers
{
    public class NoticesController : Controller
    {
        private ENoticeEntities db = new ENoticeEntities();

        // GET: Notices
        public ActionResult Index()
        {
            return View(db.Notices.ToList());
        }

        // GET: Notices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        // GET: Notices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,ImageUrl,WriterId,Type,Priority,FacultyIds,DepartmentIds,MonitorIds,CreatedAt,ModifiedAt,IsApproved")] Notice notice)
        {
            if (ModelState.IsValid)
            {
                db.Notices.Add(notice);
                db.SaveChanges();
                List<string> notifyFacultyIds = new List<string>();
                List<string> notifyDepartmentIds = new List<string>();
                List<string> notifyMonitorIds = new List<string>();

                if (notice.FacultyIds != null)
                    notifyFacultyIds = notice.FacultyIds.ToString().Split(',').ToList();
                if (notice.DepartmentIds != null)
                    notifyDepartmentIds = notice.DepartmentIds.ToString().Split(',').ToList();
                if (notice.MonitorIds != null)
                    notifyMonitorIds = notice.MonitorIds.ToString().Split(',').ToList();
                Functions.NotifyMonitors(notifyFacultyIds, notifyDepartmentIds, notifyMonitorIds, "noticeCreated: " + notice.Id);
                return RedirectToAction("Index");
            }

            return View(notice);
        }

        private static List<string> _oldFacultyIds = new List<string>();
        private static List<string> _oldDepartmentIds = new List<string>();
        private static List<string> _oldMonitorIds = new List<string>();

        // GET: Notices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            if (notice.FacultyIds != null)
                _oldFacultyIds = notice.FacultyIds.ToString().Split(',').ToList();
            if (notice.DepartmentIds != null)
                _oldDepartmentIds = notice.DepartmentIds.ToString().Split(',').ToList();
            if (notice.MonitorIds != null)
                _oldMonitorIds = notice.MonitorIds.ToString().Split(',').ToList();
            return View(notice);
        }

        // POST: Notices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,ImageUrl,WriterId,Type,Priority,FacultyIds,DepartmentIds,MonitorIds,CreatedAt,ModifiedAt,IsApproved")] Notice notice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notice).State = EntityState.Modified;
                db.SaveChanges();

                List<string> newFacultyIds = new List<string>();
                List<string> newDepartmentIds = new List<string>();
                List<string> newMonitorIds = new List<string>();

                if (notice.FacultyIds != null)
                    newFacultyIds = notice.FacultyIds.ToString().Split(',').ToList();
                if (notice.DepartmentIds != null)
                    newDepartmentIds = notice.DepartmentIds.ToString().Split(',').ToList();
                if (notice.MonitorIds != null)
                    newMonitorIds = notice.MonitorIds.ToString().Split(',').ToList();

                List<string> notifyFacultyIds = newFacultyIds;
                List<string> notifyDepartmentIds = newDepartmentIds;
                List<string> notifyMonitorIds = newMonitorIds;

                foreach (var oldFacultyId in _oldFacultyIds)
                    if (!notifyFacultyIds.Contains(oldFacultyId.Trim()))
                        notifyFacultyIds.Add(oldFacultyId);

                foreach (var oldDepartmentId in _oldDepartmentIds)
                    if (!notifyDepartmentIds.Contains(oldDepartmentId.Trim()))
                        notifyDepartmentIds.Add(oldDepartmentId);

                foreach (var oldMonitorId in _oldMonitorIds)
                    if (!notifyMonitorIds.Contains(oldMonitorId.Trim()))
                        notifyMonitorIds.Add(oldMonitorId);

                Functions.NotifyMonitors(notifyFacultyIds, notifyDepartmentIds, notifyMonitorIds, "noticeEdited: " + notice.Id);
                return RedirectToAction("Index");
            }
            return View(notice);
        }

        // GET: Notices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        // POST: Notices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notice notice = db.Notices.Find(id);
            int noticeId = notice.Id;
            List<string> facultyIds = new List<string>();
            List<string> departmentIds = new List<string>();
            List<string> monitorIds = new List<string>();

            if (notice.FacultyIds != null)
                facultyIds = notice.FacultyIds.ToString().Split(',').ToList();
            if (notice.DepartmentIds != null)
                departmentIds = notice.DepartmentIds.ToString().Split(',').ToList();
            if (notice.MonitorIds != null)
                monitorIds = notice.MonitorIds.ToString().Split(',').ToList();
            db.Notices.Remove(notice);
            db.SaveChanges();
            Functions.NotifyMonitors(facultyIds, departmentIds, monitorIds, "noticeDeleted: " + noticeId);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
