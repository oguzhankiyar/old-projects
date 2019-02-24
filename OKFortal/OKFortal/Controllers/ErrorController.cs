using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKFortal.Functions;

namespace OKFortal.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult SiteOff()
        {
            if (OK.Config("site-on/off") == "0")
                return View();
            else
                return HttpNotFound();
        }
    }
}
