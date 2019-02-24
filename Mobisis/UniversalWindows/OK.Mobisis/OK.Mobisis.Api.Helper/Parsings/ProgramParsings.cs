using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using OK.Mobisis.Entities.ApiEntities;
using OK.Mobisis.Entities.Enums;
using OK.Mobisis.Api.Helper.Enums;

namespace OK.Mobisis.Api.Helper.Parsings
{
    public static class ProgramParsings
    {
        public static RequestObject GetPrograms(string studentId, string password)
        {
            var requestObject = new RequestObject();
            requestObject.Type = RequestType.GetPrograms;
            
            var studentObject = new StudentObject();
            studentObject.Id = studentId;
            studentObject.Password = password;

            requestObject.Student = studentObject;
            return requestObject;
        }

        public static BaseResponse<List<OK.Mobisis.Entities.ModelEntities.Program>> ParsePrograms(string json)
        {
            var programResponse = new BaseResponse<List<OK.Mobisis.Entities.ModelEntities.Program>>();
            var responseObject = JsonConvert.DeserializeObject<ResultObject<List<OK.Mobisis.Entities.ModelEntities.Program>>>(json);
            if (responseObject.Response != null)
            {
                programResponse.Result = responseObject.Response;
            }
            else
            {
                programResponse.Status = ResponseStatus.Undefined;
                programResponse.Message = responseObject.Message;
            }
            return programResponse;
        }
    }
}
