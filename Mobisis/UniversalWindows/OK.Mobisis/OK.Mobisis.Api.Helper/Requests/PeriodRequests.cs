using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Entities.ModelEntities;
using OK.Mobisis.Api.Helper.Parsings;

namespace OK.Mobisis.Api.Helper.Requests
{
    public static class PeriodRequests
    {
        public static BaseRequest<List<Period>> PeriodRequest;

        static PeriodRequests()
        {
            PeriodRequest = BaseRequest<List<Period>>.GetInstance();
        }

        public static void GetPeriods(string studentId, string password, int programCode)
        {
            var requestObj = PeriodParsings.GetPeriods(studentId, password, programCode);
            PeriodRequest.OnPopulated = (jsonResult) =>
            {
                PeriodRequest.Response = PeriodParsings.ParsePeriods(jsonResult);
            };
            Global.OnRequestCalled("PeriodRequests.GetPeriods", new object[] { studentId, password, programCode });
            PeriodRequest.GetResult(requestObj);
        }
    }
}
