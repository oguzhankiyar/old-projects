using Microsoft.Web.WebSockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OK.QRAuth.Api.Models
{
    public class SocketHandler : WebSocketHandler
    {
        private Client client { get; set; }

        public SocketHandler(Client client)
        {
            this.client = client;
        }

        public override void OnOpen()
        {
            var obj = new JObject();
            obj["message"] = "hello";
            obj["token"] = client.Token;

            string message = JsonConvert.SerializeObject(obj);

            Send(message);
        }

        public override void OnClose()
        {
        }

        public override void OnMessage(string message)
        {
        }
    }
}