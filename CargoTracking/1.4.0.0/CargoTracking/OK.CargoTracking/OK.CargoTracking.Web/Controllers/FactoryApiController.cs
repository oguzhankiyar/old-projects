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
    public class FactoryApiController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> PostFactories(BaseRequest request)
        {
            try
            {
                if (!RequestHelper.VerifyRequest(request, "factory"))
                    throw new Exception();

                var response = new BaseResponse();
                var factory = request.Request as Model.Factory;
                if (factory != null)
                    response.Result = Repository.GetFactory(factory.Id);
                else
                    response.Result = Repository.GetAllFactories();
                response.Status = true;
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
