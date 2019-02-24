using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Entities.ModelEntities;
using OK.Mobisis.Api.Helper.Parsings;

namespace OK.Mobisis.Api.Helper.Requests
{
    public static class StudentRequests
    {
        public static BaseRequest<Student> StudentRequest;

        static StudentRequests()
        {
            StudentRequest = BaseRequest<Student>.GetInstance();
        }

        public static void GetStudent(string studentId, string password)
        {
            var requestObj = StudentParsings.GetStudent(studentId, password);
            StudentRequest.OnPopulated = (jsonResult) =>
            {
                StudentRequest.Response = StudentParsings.ParseStudent(jsonResult);
            };
            Global.OnRequestCalled("StudentRequests.GetStudent", new object[] { studentId, password });
            StudentRequest.GetResult(requestObj);
        }
    }
}
