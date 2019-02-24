using OK.LogManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OK.LogManagement.Controllers
{
    public class ActionViewModel
    {
        public string software { get; set; }
        public string data { get; set; }
    }

    public class HomeController : Controller
    {
        [HttpPost]
        public ActionResult Index(ActionViewModel model)
        {
            try
            {
                var db = new DbEntities();
                db.Actions.Add(new Models.Action()
                {
                    Data = model.data,
                    Software = model.software,
                    ActionDate = DateTime.Now
                });
                db.SaveChanges();
                return Json(new { status = "OK" });
            }
            catch (Exception)
            {
                return Json(new { status = "ERROR" });
            }
        }
    }
}