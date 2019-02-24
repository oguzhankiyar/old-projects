using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGK.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NoMember()
        {
            return View();
        }
        public ActionResult NoRole()
        {
            return View();
        }
        public ActionResult CustomErrors()
        {
            return View();
        }
    }
}
