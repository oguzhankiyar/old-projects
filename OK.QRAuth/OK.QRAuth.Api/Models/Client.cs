using Microsoft.Web.WebSockets;
using System;

namespace OK.QRAuth.Api.Models
{
    public class Client
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public DateTime ConnectedAt { get; set; }
        public WebSocketHandler Socket { get; set; }
    }
}