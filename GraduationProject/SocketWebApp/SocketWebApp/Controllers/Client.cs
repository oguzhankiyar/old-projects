using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.WebSockets;

namespace SocketWebApp.Controllers
{
    public class Client
    {
        public Guid Id { get; set; }
        public WebSocketHandler Socket { get; set; }
        public bool IsServer { get; set; }

        public Client(WebSocketHandler socket)
        {
            this.Id = Guid.NewGuid();
            this.Socket = socket;
        }
    }
}