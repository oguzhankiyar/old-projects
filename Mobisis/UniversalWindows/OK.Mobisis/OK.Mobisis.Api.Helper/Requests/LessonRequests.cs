using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Entities.ModelEntities;
using OK.Mobisis.Api.Helper.Parsings;

namespace OK.Mobisis.Api.Helper.Requests
{
    public static class LessonRequests
    {
        public static BaseRequest<List<Lesson>> LessonRequest;
        public static BaseRequest<List<FriendMark>> MarkListRequest;

        static LessonRequests()
        {
            LessonRequest = BaseRequest<List<Lesson>>.GetInstance();
        }

        public static void GetLessons(string studentId, string password, int periodCode, int periodYearCode)
        {
            var requestObj = LessonParsings.GetLessons(studentId, password, periodCode, periodYearCode);
            LessonRequest.OnPopulated = (jsonResult) =>
            {
                LessonRequest.Response = LessonParsings.ParseLessons(jsonResult);
            };
            Global.OnRequestCalled("LessonRequests.GetLessons", new object[] { studentId, password, periodCode, periodYearCode });
            LessonRequest.GetResult(requestObj);
        }

        public static void GetMarkList(string studentId, string password, string lessonHash)
        {
            var requestObj = LessonParsings.GetMarkList(studentId, password, lessonHash);
            MarkListRequest.OnPopulated = (jsonResult) =>
            {
                MarkListRequest.Response = LessonParsings.ParseMarkList(jsonResult);
            };
            Global.OnRequestCalled("LessonRequests.GetMarkList", new object[] { studentId, password, lessonHash });
            MarkListRequest.GetResult(requestObj);
        }
    }
}
