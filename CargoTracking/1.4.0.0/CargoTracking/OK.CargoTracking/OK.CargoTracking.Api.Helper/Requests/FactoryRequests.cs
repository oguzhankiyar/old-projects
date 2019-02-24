using Newtonsoft.Json;
using OK.CargoTracking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OK.CargoTracking.Api.Helper.Requests
{
    public static class FactoryRequests
    {
        public static async Task<BaseResponse> GetFactoriesAsync()
        {
            string postData = GetFactoriesRequestJson();
            string result = await Functions.GetHtmlAsync(Global.API_URL + "/factories", null, postData);
            var response = JsonConvert.DeserializeObject<BaseResponse>(result);
            if (response.Result != null)
                response.Result = JsonConvert.DeserializeObject<List<Factory>>(response.Result.ToString());
            return response;
        }

        private static string GetFactoriesRequestJson()
        {
            var request = new BaseRequest();
            request.Device = Global.Device;
            request.Token = Global.Token;
            return JsonConvert.SerializeObject(request);
        }

        public static async Task<BaseResponse> GetFactoryDetailsAsync(int id)
        {
            string postData = GetFactoryDetailsRequestJson(id);
            string result = await Functions.GetHtmlAsync(Global.API_URL + "/factories", null, postData);
            var response = JsonConvert.DeserializeObject<BaseResponse>(result);
            if (response.Result != null)
                response.Result = JsonConvert.DeserializeObject<Factory>(response.Result.ToString());
            return response;
        }

        private static string GetFactoryDetailsRequestJson(int id)
        {
            var request = new BaseRequest();
            request.Request = new Factory() { Id = id };
            request.Device = Global.Device;
            request.Token = Global.Token;
            return JsonConvert.SerializeObject(request);
        }
    }
}
