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
    public static class GraduationParsings
    {
        public static RequestObject GetGraduation(string studentId, string password)
        {
            var requestObject = new RequestObject();
            requestObject.Type = RequestType.GetGraduation;

            var studentObject = new StudentObject();
            studentObject.Id = studentId;
            studentObject.Password = password;

            requestObject.Student = studentObject;
            return requestObject;
        }

        public static BaseResponse<List<Graduation>> ParseGraduation(string json)
        {
            var graduationResponse = new BaseResponse<List<Graduation>>();
            var responseObject = JsonConvert.DeserializeObject<ResultObject<List<Graduation>>>(json);
            if (responseObject.Response != null)
            {
                graduationResponse.Result = responseObject.Response;
            }
            else
            {
                graduationResponse.Status = ResponseStatus.Undefined;
                graduationResponse.Message = responseObject.Message;
            }
            return graduationResponse;
        }
    }
}
