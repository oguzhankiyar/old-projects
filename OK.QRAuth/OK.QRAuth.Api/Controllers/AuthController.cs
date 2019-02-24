using Microsoft.Web.WebSockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OK.QRAuth.Api.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OK.QRAuth.Api.Controllers
{
    public class AuthController : ApiController
    {
        public HttpResponseMessage Get()
        {
            Client client = new Client();
            client.Token = Guid.NewGuid().ToString().Replace("-", "");
            client.Socket = new SocketHandler(client);
            ClientList.Insert(client);

            HttpContext.Current.AcceptWebSocketRequest(client.Socket);
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        public string Post(ApiModel model)
        {
            var result = new JObject();
            result["status"] = false;

            var client = ClientList.Find(model.Token);
            if (client != null)
            {
                var obj = new JObject();
                obj["message"] = "completed";
                obj["token"] = model.AccessToken;

                string message = JsonConvert.SerializeObject(obj);

                client.Socket.Send(message);

                ClientList.Delete(client.Id);

                result["status"] = true;
            }

            return JsonConvert.SerializeObject(result);
        }
    }
}