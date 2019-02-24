using Newtonsoft.Json;
using OK.CargoTracking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OK.CargoTracking.Api.Helper.Requests
{
    public static class DeviceRequests
    {
        public static async Task<BaseResponse> RegisterDeviceAsync()
        {
            string postData = GetRegisterDeviceRequestJson();
            string result = await Functions.GetHtmlAsync(Global.API_URL + "/device", null, postData);
            return JsonConvert.DeserializeObject<BaseResponse>(result);
        }

        private static string GetRegisterDeviceRequestJson()
        {
            var request = new BaseRequest();
            request.Device = Global.Device;
            request.Token = Global.Token;
            return JsonConvert.SerializeObject(request);
        }
    }
}
