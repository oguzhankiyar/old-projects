using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OKPort.Models;

namespace OKPort.Controllers
{
    public class HomeController : Controller
    {
        OKDbEntities Db = new OKDbEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string query)
        {
            var pages = Db.Pages.Where(x => x.Name.Contains(query));
            return View(pages);
        }
        public ActionResult OKML()
        {
            return View();
        }
        public string decode(string code)
        {
            return OKPort.Functions.OKParser.GetHtml(code);
        }
    }
}