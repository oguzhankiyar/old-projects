using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Entities.ModelEntities;
using OK.Mobisis.Api.Helper.Parsings;

namespace OK.Mobisis.Api.Helper.Requests
{
    public static class GraduationRequests
    {
        public static BaseRequest<List<Graduation>> GraduationRequest;

        static GraduationRequests()
        {
            GraduationRequest = BaseRequest<List<Graduation>>.GetInstance();
        }

        public static void GetGraduation(string studentId, string password)
        {
            var requestObj = GraduationParsings.GetGraduation(studentId, password);
            GraduationRequest.OnPopulated = (jsonResult) =>
            {
                GraduationRequest.Response = GraduationParsings.ParseGraduation(jsonResult);
            };
            Global.OnRequestCalled("GraduationRequests.GetGraduation", new object[] { studentId, password });
            GraduationRequest.GetResult(requestObj);
        }
    }
}
