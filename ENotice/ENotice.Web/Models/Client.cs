using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.WebSockets;

namespace ENotice.Web.Models
{
    public class Client
    {
        public int MonitorId { get; set; }
        public WebSocketHandler Socket { get; set; }

        public Client(int monitorId, WebSocketHandler socket)
        {
            this.MonitorId = monitorId;
            this.Socket = socket;
        }
    }
}