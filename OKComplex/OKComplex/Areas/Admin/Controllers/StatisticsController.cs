using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OKComplex.Functions;

namespace OKComplex.Areas.Admin.Controllers
{
    [AdminControl]
    public class StatisticsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
