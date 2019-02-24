using Newtonsoft.Json;
using OK.CargoTracking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OK.CargoTracking.Api.Helper.Requests
{
    public static class MessageRequests
    {
        public static async Task<BaseResponse> GetMessagesAsync()
        {
            string postData = GetMessagesRequestJson();
            string result = await Functions.GetHtmlAsync(Global.API_URL + "/messages", null, postData);
            var response = JsonConvert.DeserializeObject<BaseResponse>(result);
            if (response.Result != null)
                response.Result = JsonConvert.DeserializeObject<List<Message>>(response.Result.ToString());
            return response;
        }

        private static string GetMessagesRequestJson()
        {
            var request = new BaseRequest();
            request.Device = Global.Device;
            request.Token = Global.Token;
            return JsonConvert.SerializeObject(request);
        }
    }
}
