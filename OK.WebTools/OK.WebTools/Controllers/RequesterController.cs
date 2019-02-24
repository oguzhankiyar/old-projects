using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OK.WebTools.Controllers
{
    public class RequestForm
    {
        public string Url { get; set; }
        public string Result { get; set; }
    }
    public class RequesterController : Controller
    {
        // GET: Requester
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(RequestForm model)
        {
            model.Url = "http://" + model.Url.Replace("http://", "");
            var request = HttpWebRequest.Create(model.Url) as HttpWebRequest;
            var response = request.GetResponse();
            var sr = new StreamReader(response.GetResponseStream());
            model.Result = sr.ReadToEnd();
            model.Url = model.Url.Replace("http://", "");
            return View(model);
        }
    }
}