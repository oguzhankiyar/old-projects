﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ENotice.Web.Models;
using Microsoft.Web.WebSockets;

namespace ENotice.Web.Controllers
{
    public class SocketApiController : ApiController
    {
        public HttpResponseMessage Get()
        {
            HttpContext.Current.AcceptWebSocketRequest(new SocketHandler());
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }
    }
}
