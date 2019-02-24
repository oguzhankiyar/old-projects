using BGK.Functions;
using BGK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGK.Areas.Admin.Controllers
{
    public class SharedController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Menu()
        {
            return PartialView();
        }
        public ActionResult DeleteActions()
        {
            return PartialView();
        }
    }
}
