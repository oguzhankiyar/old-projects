using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using BGK.Functions;

namespace BGK.Areas.Admin.Controllers
{
    [Controls("MissionManagement")]
    public class MissionController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
