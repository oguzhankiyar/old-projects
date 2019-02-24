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
    public class DeviceApiController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> PostRegister(BaseRequest request)
        {
            try
            {
                if (!RequestHelper.VerifyRequest(request, "device"))
                    throw new Exception();

                var device = Repository.AddDevice(request.Device.Id, request.Device.Name, request.Device.Platform, request.Device.AppVersion);
                if (device != null)
                    return Json(new BaseResponse() { Status = true });
                return Json(new BaseResponse() { Status = false });
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
