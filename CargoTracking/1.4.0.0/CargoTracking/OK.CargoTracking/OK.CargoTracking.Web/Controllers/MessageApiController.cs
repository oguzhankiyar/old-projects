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
    public class MessageApiController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> PostCheck(BaseRequest request)
        {
            var response = new BaseResponse();
            try
            {
                if (!RequestHelper.VerifyRequest(request, "message"))
                    throw new Exception();

                response.Result = Repository.GetDeviceMessages(request.Device.Id);
                response.Status = true;
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
                response.Message = message;
                response.Status = false;
            }
            return Json(response);
            
            
            /* 
            return Json(
            new BaseResponse()
            {
                Status = true,
                Result = new List<Message>() {
                    new Message() {
                        Type = "raw",
                        Title = "Yeni Güncelleme Yayınlandı!",
                        Content = "Merhaba Kargo Takip kullanıcısı,\nuygulamamız 1.3.0.0 sürümüne güncellendi. Yeni sürüm ile birlikte bazı hatalar giderildi ve performans iyileştirmeleri yapıldı.\n\nDaha iyi bir deneyim için güncelleyin.",
                        Buttons = new List<MessageButton>() {
                            new MessageButton("Ara", "track(1,5419551126670)"),
                            new MessageButton("Daha Sonra", "closeMessage()")
                        }
                    },
                    new Message() {
                        Type = "xaml",
                        Title = "Biletonly: Otobüs ve Uçak Bileti",
                        Content = "<Grid xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Image Source=\"http://biletonly.com/Content/TicketApp.MediumLarge.png\" Height=\"50\" /></Grid>",
                        Buttons = new List<MessageButton>() {
                            new MessageButton("Hemen İndir", "launcher(zune:navigate?appid=cd14f22b-23bc-4ff1-81b6-849d77e6ec1c)"),
                            new MessageButton("Daha Sonra", "closeMessage()")
                        }
                    },
                    new Message() {
                        Type = "xaml",
                        Title = "Kargo Takip'i Oylayın",
                        Content = "<Grid xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Image Source=\"http://biletonly.com/Content/TicketApp.MediumLarge.png\" Height=\"50\" /></Grid>",
                        Buttons = new List<MessageButton>() {
                            new MessageButton("Oyla", "launcher(ms-windows-store:reviewapp?appid=cd14f22b-23bc-4ff1-81b6-849d77e6ec1c)"),
                            new MessageButton("Daha Sonra", "closeMessage()")
                        }
                    }
                }
            });*/
        }
    }
}
