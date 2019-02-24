using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace SocketWebApp.Controllers
{
    public class Result
    {
        public bool Status { get; set; }
    }
    public class SendController : ApiController
    {
        public IHttpActionResult Get(string message)
        {
            Functions.SendAll(message);
            return Json(new Result { Status = true});
        }
    }
}
