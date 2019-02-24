using OKComplex.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OKComplex.Areas.Panel.Controllers
{
    [PanelControl]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
