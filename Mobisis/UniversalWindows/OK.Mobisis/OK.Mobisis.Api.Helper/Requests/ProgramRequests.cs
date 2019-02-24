using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Api.Helper.Parsings;

namespace OK.Mobisis.Api.Helper.Requests
{
    public static class ProgramRequests
    {
        public static BaseRequest<List<OK.Mobisis.Entities.ModelEntities.Program>> ProgramRequest;

        static ProgramRequests()
        {
            ProgramRequest = BaseRequest<List<OK.Mobisis.Entities.ModelEntities.Program>>.GetInstance();
        }

        public static void GetPrograms(string studentId, string password)
        {
            var requestObj = ProgramParsings.GetPrograms(studentId, password);
            ProgramRequest.OnPopulated = (jsonResult) =>
            {
                ProgramRequest.Response = ProgramParsings.ParsePrograms(jsonResult);
            };
            Global.OnRequestCalled("ProgramRequests.GetPrograms", new object[] { studentId, password });
            ProgramRequest.GetResult(requestObj);
        }
    }
}
