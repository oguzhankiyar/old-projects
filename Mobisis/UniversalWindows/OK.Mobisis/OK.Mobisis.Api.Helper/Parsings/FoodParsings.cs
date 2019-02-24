using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using OK.Mobisis.Entities.ApiEntities;
using OK.Mobisis.Entities.Enums;
using OK.Mobisis.Entities.ModelEntities;
using OK.Mobisis.Api.Helper.Enums;

namespace OK.Mobisis.Api.Helper.Parsings
{
    public static class FoodParsings
    {
        public static RequestObject GetFoodLists()
        {
            var requestObject = new RequestObject();
            requestObject.Type = RequestType.GetFoodLists;
            return requestObject;
        }

        public static BaseResponse<List<FoodList>> ParseFoodLists(string json)
        {
            var foodResponse = new BaseResponse<List<FoodList>>();
            var responseObject = JsonConvert.DeserializeObject<ResultObject<List<FoodList>>>(json);
            if (responseObject.Response != null)
            {
                foodResponse.Result = responseObject.Response;
            }
            else
            {
                foodResponse.Status = ResponseStatus.Undefined;
                foodResponse.Message = responseObject.Message;
            }
            return foodResponse;
        }
    }
}
