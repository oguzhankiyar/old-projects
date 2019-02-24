using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialObisis.Models;

namespace SocialObisis.Controllers
{
    public class FacultyController : Controller
    {
        public ActionResult Index()
        {
            Obisis.Ogrenci ogr = (Obisis.Ogrenci)Session["student"];
            return View(ogr);
        }
    }
}
