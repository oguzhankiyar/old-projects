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
    public static class DocumentParsings
    {
        public static RequestObject GetDocuments(string studentId, string password)
        {
            var requestObject = new RequestObject();
            requestObject.Type = RequestType.GetDocuments;

            var studentObject = new StudentObject();
            studentObject.Id = studentId;
            studentObject.Password = password;

            requestObject.Student = studentObject;
            return requestObject;
        }

        public static BaseResponse<List<Document>> ParseDocuments(string json)
        {
            var documentResponse = new BaseResponse<List<Document>>();
            var responseObject = JsonConvert.DeserializeObject<ResultObject<List<Document>>>(json);
            if (responseObject.Response != null)
            {
                documentResponse.Result = responseObject.Response;
            }
            else
            {
                documentResponse.Status = ResponseStatus.Undefined;
                documentResponse.Message = responseObject.Message;
            }
            return documentResponse;
        }
    }
}
