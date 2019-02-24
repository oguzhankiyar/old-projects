using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OK.Mobisis.Entities.ApiEntities;
using OK.Mobisis.Api.Helper.Enums;
using OK.Mobisis.Entities.Enums;
using OK.Mobisis.Entities.ModelEntities;

namespace OK.Mobisis.Api.Helper.Parsings
{
    public static class StudentParsings
    {
        public static RequestObject GetStudent(string studentId, string password)
        {
            var requestObject = new RequestObject();
            requestObject.Type = RequestType.GetStudent;
            
            var studentObject = new StudentObject();
            studentObject.Id = studentId;
            studentObject.Password = password;

            requestObject.Student = studentObject;
            return requestObject;
        }

        public static BaseResponse<Student> ParseStudent(string json)
        {
            var studentResponse = new BaseResponse<Student>();
            var responseObject = JsonConvert.DeserializeObject<ResultObject<Student>>(json);
            if (responseObject.Response != null)
            {
                studentResponse.Result = responseObject.Response;
                studentResponse.Message = responseObject.Message;
            }
            else
            {
                studentResponse.Status = ResponseStatus.Undefined;
                studentResponse.Message = responseObject.Message;
            }
            return studentResponse;
        }

    }
}
