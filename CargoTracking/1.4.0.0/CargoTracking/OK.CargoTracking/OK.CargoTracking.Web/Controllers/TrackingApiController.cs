using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OK.CargoTracking.Helper;
using OK.CargoTracking.Model;
using OK.CargoTracking.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OK.CargoTracking.Web.Controllers
{
    public class TrackingApiController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> PostSearch(BaseRequest req)
        {
            try
            {
                if (!RequestHelper.VerifyRequest(req, "tracking"))
                    throw new Exception();

                var search = JsonConvert.DeserializeObject<TrackingSearch>(req.Request.ToString());

                var factory = Repository.GetFactory(search.FactoryId);

                var response = await TrackingRequest.GetResponseAsync(search.Code, factory);
                Repository.AddAction(search.Code, factory.Id, response.Status, req.Device.Id);
                if (!response.Status)
                {
                    var message = new Model.Message();
                    message.Title = "Bulunamadı";
                    message.Content = "Aradığınız kod ile eşleşen sonuç bulunamadı veya bir sorun oluştu.\nSorunu çözmeye çalışacağız.";
                    message.Type = "raw";
                    message.Buttons = new List<Model.MessageButton>()
                {
                    new Model.MessageButton("Tekrar dene", "track(" + search.FactoryId + ", " + search.Code + ")"),
                    new Model.MessageButton("Daha Sonra", "closeMessage()")
                };
                }
                return Json(response);
            }
            catch (Exception)
            {
                var message = new Model.Message();
                message.Title = "Bir sorun oluştu.";
                message.Content = "Lütfen daha sonra tekrar deneyiniz.";
                message.Type = "raw";
                message.Buttons = new List<Model.MessageButton>()
                {
                    new Model.MessageButton("Tamam", "closeMessage()")
                };
                return Json(new BaseResponse() { Status = false, Message = message });
            }
        }
    }
}
