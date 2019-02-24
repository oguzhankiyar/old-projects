using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Functions;

namespace OKComplex.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult SiteOff()
        {
            if (OK.ClubConfig("site-on/off") == "0")
                return View();
            else
                return HttpNotFound();
        }
    }
}
