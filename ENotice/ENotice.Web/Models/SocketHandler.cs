using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.WebSockets;

namespace ENotice.Web.Models
{
    public class SocketHandler : WebSocketHandler
    {
        //public static WebSocketCollection ConnectedClients;
        //public static List<Client> ClientList;
        //private int _monitorId;
        /*
        static SocketHandler()
        {
            //ConnectedClients = new WebSocketCollection();
            //ClientList = new List<Client>();
        }
        */
        public override void OnOpen()
        {
            base.OnOpen();
            Functions.ClientOpened(this);
            //ConnectedClients.Add(this);
            //ConnectedClients.Broadcast(_monitorId + " is joined");
        }

        public override void OnClose()
        {
            base.OnClose();
            Functions.ClientClosed(this);
            //ClientList.Remove(ClientList.SingleOrDefault(x => x.Socket == this));
            //Functions.SendServerMessage(_monitorId + " is left");
            
            //ConnectedClients.Remove(this);
            //ConnectedClients.Broadcast(_monitorId + " is left");
        }

        public override void OnMessage(string message)
        {
            Functions.ClientMessaged(this, message);
            //Functions.SendServerMessage(_monitorId + ": " + message);
           
            //ConnectedClients.Broadcast(_monitorId + " : " + message);
        }
    }
}