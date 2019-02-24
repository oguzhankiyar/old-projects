using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocketWebApp.Controllers
{
    public class Functions
    {
        public static List<Client> Clients;

        static Functions()
        {
            Clients = new List<Client>();
        }

        public static void SendAll(string message)
        {
            foreach (var client in Clients)
            {
                client.Socket.Send(message);
            }
        }

        public static void SendMessage(string id, string message)
        {
            foreach (var item in Clients.Where(x => x.Id.ToString() == id))
            {
                item.Socket.Send(message);
            }
        }

        public static void SendServerMessage(string message)
        {
            foreach (var item in Clients.Where(x => x.IsServer))
            {
                item.Socket.Send(message);
            }
        }

        public static void ClientOpened(SocketHandler socketHandler)
        {
            //socketHandler.WebSocketContext.QueryString
            var client = new Client(socketHandler);
            Clients.Add(client);
            Functions.SendServerMessage(" is joined");
            socketHandler.Send("Welcome");
        }

        public static void ClientMessaged(SocketHandler socketHandler, string message)
        {
            Functions.SendServerMessage(": " + message);
            socketHandler.Send("You sent: " + message);
        }

        public static void ClientClosed(SocketHandler socketHandler)
        {
            Clients.RemoveAll(x => x.Socket == socketHandler);
            Functions.SendServerMessage(" is left");
            socketHandler.Send("Good Bye");
        }
    }
}