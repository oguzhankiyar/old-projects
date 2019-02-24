using OKFortal.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OKFortal.Areas.Admin.Controllers
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
