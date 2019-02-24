using Newtonsoft.Json;
using OK.CargoTracking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OK.CargoTracking.Api.Helper.Requests
{
    public static class TrackingRequests
    {
        public static async Task<BaseResponse> GetDetailsAsync(Factory factory, string code)
        {
            string postData = GetDetailsRequestJson(factory, code);
            string result = await Functions.GetHtmlAsync(Global.API_URL + "/tracking", null, postData);
            var response = JsonConvert.DeserializeObject<BaseResponse>(result);
            if (response.Result != null)
                response.Result = JsonConvert.DeserializeObject<Tracking>(response.Result.ToString());
            return response;
        }

        private static string GetDetailsRequestJson(Factory factory, string code)
        {
            var request = new BaseRequest();
            request.Device = Global.Device;
            request.Token = Global.Token;

            var trackingRequest = new TrackingSearch();
            trackingRequest.FactoryId = factory.Id;
            trackingRequest.Code = code;

            request.Request = trackingRequest;
            return JsonConvert.SerializeObject(request);
        }
    }
}
