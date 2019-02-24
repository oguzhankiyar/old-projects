using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OK.Mobisis.Api.Helper;
using OK.Mobisis.Api.Helper.Enums;
using OK.Mobisis.Api.Helper.Parsings;
using OK.Mobisis.Api.Helper.Requests;
using OK.Mobisis.Entities.ModelEntities;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;

namespace OK.Mobisis.BackgroundTasks
{
    public sealed class UpdatePeriodTask : IBackgroundTask
    {
        private Student currentStudent;
        private JObject obj;
        private BackgroundTaskDeferral deferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            deferral = taskInstance.GetDeferral();

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                var saver = new DataSaver();
                saver.ReadCompleted = (json) =>
                {
                    obj = JObject.Parse(json);

                    Global.ApiUsername = obj["Settings"]["ApiUsername"].ToString();
                    Global.ApiPassword = obj["Settings"]["ApiPassword"].ToString();
                    if (Global.Invoke == null)
                        Global.Invoke = (action) => { action(); };

                    currentStudent = JsonConvert.DeserializeObject<Student>(obj["Student"].ToString());

                    if (currentStudent != null)
                    {
                        var currentPeriod = currentStudent.Programs[0].Periods[0];

                        LessonRequests.LessonRequest.OnCompleted = GetLessons_Completed;
                        LessonRequests.GetLessons(currentStudent.Id, currentStudent.Password, currentPeriod.Code, currentPeriod.YearCode);
                    }
                };
                saver.ReadAsync("SavedData");
            }
            else
                deferral.Complete();
        }

        private async void GetLessons_Completed(BaseResponse<List<Lesson>> lessonResponse)
        {
            bool isCompleted = true;
            if (lessonResponse.Status == ResponseStatus.Successful)
            {
                isCompleted = false;
                var lessons = lessonResponse.Result;
                foreach (var oldLesson in currentStudent.Programs[0].Periods[0].Lessons)
                {
                    var newLesson = lessons.SingleOrDefault(x => x.Name == oldLesson.Name);
                    if (newLesson != null)
                    {
                        string text = "";
                        bool isNew = false;
                        if (newLesson.FirstMidterm.Mark != oldLesson.FirstMidterm.Mark && newLesson.FirstMidterm.Mark != null)
                        {
                            text += "1. Vize: " + newLesson.FirstMidterm.Mark + " ";
                            isNew = true;
                        }
                        if (newLesson.SecondMidterm.Mark != oldLesson.SecondMidterm.Mark && newLesson.SecondMidterm.Mark != null)
                        {
                            text += "2. Vize: " + newLesson.SecondMidterm.Mark + " ";
                            isNew = true;
                        }
                        if (newLesson.ThirdMidterm.Mark != oldLesson.ThirdMidterm.Mark && newLesson.ThirdMidterm.Mark != null)
                        {
                            text += "3. Vize: " + newLesson.ThirdMidterm.Mark + " ";
                            isNew = true;
                        }
                        if (newLesson.Final.Mark != oldLesson.Final.Mark && newLesson.Final.Mark != null)
                        {
                            text += "Final: " + newLesson.Final.Mark + " ";
                            isNew = true;
                        }
                        if (newLesson.Integration.Mark != oldLesson.Integration.Mark && newLesson.Integration.Mark != null)
                        {
                            text += "Bütünleme: " + newLesson.Integration.Mark + " ";
                            isNew = true;
                        }
                        if (newLesson.Average != oldLesson.Average && newLesson.Average != null)
                        {
                            text += "Ortalama: " + newLesson.Average + " ";
                            isNew = true;
                        }
                        if (!isNew)
                            text = text.Replace(newLesson.Name + " ", "");

                        if (!string.IsNullOrEmpty(text))
                        {
                            ShowToastNotification(oldLesson.Name, text);
                        }
                    }
                }
                currentStudent.Programs[0].Periods[0].Lessons = lessons;
                obj["Student"] = JObject.Parse(JsonConvert.SerializeObject(currentStudent));
                var saver = new DataSaver();
                saver.SaveCompleted = (result) =>
                {
                    deferral.Complete();
                };
                saver.SaveAsync("SavedData", obj.ToString());
            }
            if (isCompleted)
                deferral.Complete();
        }

        private void ShowToastNotification(string title, string text)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            XmlNodeList toastTextElement = toastXml.GetElementsByTagName("text");
            toastTextElement[0].AppendChild(toastXml.CreateTextNode(title));
            toastTextElement[1].AppendChild(toastXml.CreateTextNode(text));

            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "short");

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
