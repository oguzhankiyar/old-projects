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
    public static class LessonParsings
    {
        public static RequestObject GetLessons(string studentId, string password, int periodCode, int periodYearCode)
        {
            var requestObject = new RequestObject();
            requestObject.Type = RequestType.GetLessons;

            var periodObject = new PeriodObject();
            periodObject.StudentId = studentId;
            periodObject.Password = password;
            periodObject.Code = periodCode;
            periodObject.YearCode = periodYearCode;

            requestObject.Period = periodObject;
            return requestObject;
        }

        public static BaseResponse<List<Lesson>> ParseLessons(string json)
        {
            var lessonResponse = new BaseResponse<List<Lesson>>();
            var responseObject = JsonConvert.DeserializeObject<ResultObject<List<Lesson>>>(json);
            if (responseObject.Response != null)
            {
                lessonResponse.Result = responseObject.Response;
            }
            else
            {
                lessonResponse.Status = ResponseStatus.Undefined;
                lessonResponse.Message = responseObject.Message;
            }
            return lessonResponse;
        }

        public static RequestObject GetMarkList(string studentId, string password, string lessonHash)
        {
            var requestObject = new RequestObject();
            requestObject.Type = RequestType.GetMarkList;

            var lessonObject = new LessonObject();
            lessonObject.StudentId = studentId;
            lessonObject.Password = password;
            lessonObject.Hash = lessonHash;

            requestObject.Lesson = lessonObject;
            return requestObject;
        }

        public static BaseResponse<List<FriendMark>> ParseMarkList(string json)
        {
            var markListResponse = new BaseResponse<List<FriendMark>>();
            var responseObject = JsonConvert.DeserializeObject<ResultObject<List<FriendMark>>>(json);
            if (responseObject.Response != null)
            {
                markListResponse.Result = responseObject.Response;
            }
            else
            {
                markListResponse.Status = ResponseStatus.Undefined;
                markListResponse.Message = responseObject.Message;
            }
            return markListResponse;
        }
    }
}
