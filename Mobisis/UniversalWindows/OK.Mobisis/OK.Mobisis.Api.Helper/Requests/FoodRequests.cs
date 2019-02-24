using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Entities.ModelEntities;
using OK.Mobisis.Api.Helper.Parsings;

namespace OK.Mobisis.Api.Helper.Requests
{
    public static class FoodRequests
    {
        public static BaseRequest<List<FoodList>> FoodRequest;

        static FoodRequests()
        {
            FoodRequest = BaseRequest<List<FoodList>>.GetInstance();
        }

        public static void GetFoodLists()
        {
            var requestObj = FoodParsings.GetFoodLists();
            FoodRequest.OnPopulated = (jsonResult) =>
            {
                FoodRequest.Response = FoodParsings.ParseFoodLists(jsonResult);
            };
            Global.OnRequestCalled("FoodRequests.GetFoodLists", null);
            FoodRequest.GetResult(requestObj);
        }
    }
}
