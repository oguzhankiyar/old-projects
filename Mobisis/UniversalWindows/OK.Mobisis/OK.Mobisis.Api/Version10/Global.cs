using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OK.Mobisis.Api.Models;
using OK.Mobisis.Entities.ApiEntities;
using OK.Mobisis.Entities.Enums;
using OK.Mobisis.Entities.ModelEntities;

namespace OK.Mobisis.Api.Version10
{
    public class Global
    {
        private static MobisisEntities db = new MobisisEntities();

        public static object Execute(ApiObject obj)
        {
            if ((obj.Username == "OK.Mobisis.WindowsPhone" && obj.Password == "3f6B302c") ||
                obj.Username == "OK.Mobisis.Windows" && obj.Password == "3f6B302c" ||
                obj.Username == "OK.Mobisis.Android" && obj.Password == "3f6B302c")
            {
                var requestObj = obj.Request;
                switch (requestObj.Type)
                {
                    case RequestType.GetStudent:
                        var resultObj = new ResultObject<Student>();
                        OK.Mobisis.Api.Models.Action action = new OK.Mobisis.Api.Models.Action();
                        action.ApiUsername = obj.Username;
                        try
                        {
                            action.StudentId = requestObj.Student.Id;
                            action.Password = requestObj.Student.Password;
                            action.Date = DateTime.Now;
                            action.IsSuccessful = true;
                        }
                        catch (Exception) { }
                        Student student = null;
                        try
                        {
                            student = Helper.GetStudent(requestObj.Student);
                            resultObj.Response = student;
                        }
                        catch (Exception)
                        {
                            action.IsSuccessful = false;
                            resultObj.Response = null;
                            resultObj.Status = false;
                            resultObj.Message = "Giriş Başarısız!";
                        }
                        try
                        {
                            if (student != null)
                            {
                                action.Name = student.Name;
                                if (db.Actions.Count(x => x.StudentId == student.Id) == 0)
                                    resultObj.Message = "Merhaba, " + student.Name.Split(' ')[0];
                            }
                            db.Actions.Add(action);
                            db.SaveChanges();
                        }
                        catch { }
                        return resultObj;
                    case RequestType.GetPrograms:
                        return new ResultObject<List<Program>>() { Response = Helper.GetPrograms(requestObj.Student) };
                    case RequestType.GetPeriods:
                        return new ResultObject<List<Period>>() { Response = Helper.GetPeriods(requestObj.Program) };
                    case RequestType.GetLessons:
                        return new ResultObject<List<Lesson>>() { Response = Helper.GetLessons(requestObj.Period) };
                    case RequestType.GetMarkList:
                        return new ResultObject<List<FriendMark>>() { Response = Helper.GetMarkList(requestObj.Lesson) };
                    case RequestType.GetFoodLists:
                        return new ResultObject<List<FoodList>>() { Response = Helper.GetFoodLists() };
                    case RequestType.GetDocuments:
                        return new ResultObject<List<Document>>() { Response = Helper.GetDocuments(requestObj.Student) };
                    case RequestType.GetGraduation:
                        return new ResultObject<List<Graduation>>() { Response = Helper.GetGraduation(requestObj.Student) };
                    default:
                        return new ResultObject<String>() { Message = "Undefined request" };
                }
            }
            return new ResultObject<String>() { Status = false, Message = "Unauthorized user" };
        }
    }
}