using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ENotice.Web.Models;

namespace ENotice.Web.Controllers
{
    public class NoticesApiController : ApiController
    {
        private ENoticeEntities db = new ENoticeEntities();

        // GET: api/NoticesApi
        public IQueryable<Notice> GetNotices()
        {
            var notices = new List<Notice>();
            var queryValues = Request.RequestUri.ParseQueryString();
            if (queryValues != null || queryValues["monitorId"] != null)
            {
                int monitorId = Convert.ToInt32(queryValues["monitorId"].ToString());
                var monitor = db.Monitors.SingleOrDefault(x => x.Id == monitorId);
                if (monitor != null)
                {
                    var department = db.Departments.SingleOrDefault(x => x.Id == monitor.DepartmentId);
                    var faculty = db.Faculties.Single(x => x.Id == department.FacultyId);

                    foreach (var notice in db.Notices.ToList())
                    {
                        if (notice.IsApproved)
                        {
                            List<string> facultyIds = new List<string>();
                            List<string> departmentIds = new List<string>();
                            List<string> monitorIds = new List<string>();

                            if (notice.FacultyIds != null)
                                facultyIds = notice.FacultyIds.ToString().Split(',').ToList();

                            if (notice.DepartmentIds != null)
                                departmentIds = notice.DepartmentIds.ToString().Split(',').ToList();

                            if (notice.MonitorIds != null)
                                monitorIds = notice.MonitorIds.ToString().Split(',').ToList();

                            if (facultyIds.Contains(faculty.Id.ToString().Trim()) || departmentIds.Contains(department.Id.ToString().Trim()) || monitorIds.Contains(monitor.Id.ToString().Trim()))
                            {
                                if (!notices.Contains(notice))
                                    notices.Add(notice);
                            }
                        }
                    }
                }
            }
            return notices.AsQueryable();
        }

        // GET: api/NoticesApi/5
        [ResponseType(typeof(Notice))]
        public IHttpActionResult GetNotice(int id)
        {
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return NotFound();
            }

            return Ok(notice);
        }

        // PUT: api/NoticesApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNotice(int id, Notice notice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notice.Id)
            {
                return BadRequest();
            }

            db.Entry(notice).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/NoticesApi
        [ResponseType(typeof(Notice))]
        public IHttpActionResult PostNotice(Notice notice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Notices.Add(notice);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = notice.Id }, notice);
        }

        // DELETE: api/NoticesApi/5
        [ResponseType(typeof(Notice))]
        public IHttpActionResult DeleteNotice(int id)
        {
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return NotFound();
            }

            db.Notices.Remove(notice);
            db.SaveChanges();

            return Ok(notice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NoticeExists(int id)
        {
            return db.Notices.Count(e => e.Id == id) > 0;
        }
    }
}