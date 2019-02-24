using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SuperSocket.ClientEngine;
using WebSocket4Net;

namespace SocketClientApp
{
    class Program
    {
        private static WebSocket ws;

        static void Main(string[] args)
        {
            string host = "ws://socket.ogzkyr.net/api/socketapi";

            ws = new WebSocket(host);
            ws.Closed += new EventHandler(ws_Closed);
            ws.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(ws_Error);
            ws.MessageReceived += new EventHandler<MessageReceivedEventArgs>(ws_MessageReceived);
            ws.Opened += new EventHandler(ws_Opened);
            ws.Open();
            
            Console.ReadKey();
        }
        

        private static void ws_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("opened");
        }

        private static void ws_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("received: " + e.Message);
        }

        private static void ws_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("error: " + e.Exception.Message);
        }

        private static void ws_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("closed");
        }
    }
}
