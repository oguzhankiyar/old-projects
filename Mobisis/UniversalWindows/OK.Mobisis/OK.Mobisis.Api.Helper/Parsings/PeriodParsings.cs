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
    public static class PeriodParsings
    {
        public static RequestObject GetPeriods(string studentId, string password, int programCode)
        {
            var requestObject = new RequestObject();
            requestObject.Type = RequestType.GetPeriods;

            var programObject = new ProgramObject();
            programObject.StudentId = studentId;
            programObject.Password = password;
            programObject.Code = programCode;

            requestObject.Program = programObject;
            return requestObject;
        }

        public static BaseResponse<List<Period>> ParsePeriods(string json)
        {
            var periodResponse = new BaseResponse<List<Period>>();
            var responseObject = JsonConvert.DeserializeObject<ResultObject<List<Period>>>(json);
            if (responseObject.Response != null)
            {
                periodResponse.Result = responseObject.Response;
            }
            else
            {
                periodResponse.Status = ResponseStatus.Undefined;
                periodResponse.Message = responseObject.Message;
            }
            return periodResponse;
        }
    }
}
