using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using ObisisService.ErciyesWebService;
using HtmlAgilityPack;

namespace ObisisService.Models
{
    [DataContract]
    public class Connection : ObjectModel
    {
        [DataMember]
        public bool State { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public ObjectModel DataObject { get; set; }

        public Connection GetStudent(int studentID, string password)
        {
            try
            {
                Data.StudentID = studentID;
                Data.Password = password;
                Student student = new Student();
                string html = Function.GetHTML("http://obisis.erciyes.edu.tr", studentID, password);
                Data.GetURLs(html);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNode node = doc.DocumentNode;
                student.ID = studentID;
                student.Password = password;
                student.Name = Function.TidyText(node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtAdiSoyadi']").InnerText);
                student.Programs = Program.GetPrograms(html);
                student.GraduationState = Graduation.GetGraduationState(Function.GetHTML(Data.GraduationURL, studentID, password));
                student.Documents = Document.GetDocuments(Function.GetHTML(Data.DocumentsURL, studentID, password));

                this.State = true;
                this.Message = "Merhaba " + student.Name;
                this.DataObject = student;
            }
            catch (Exception)
            {
                this.State = false;
                this.Message = "Giriş başarısız!";
                this.DataObject = null;
            }
            return this;
        }
        public Connection GetCurrentPeriod(Period currentPeriod, int studentID, string password)
        {
            try
            {
                Data.StudentID = studentID;
                Data.Password = password;
                string baseHtml = Function.GetHTML("http://obisis.erciyes.edu.tr/");
                Data.GetURLs(baseHtml);
                string html = Function.GetHTML(Data.LessonsURL);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNode node = doc.DocumentNode;
                HtmlNodeCollection col = node.SelectNodes("//table[@id='ctl00_dgDers']//tr");
                for (int i = 1; i < col.Count; i++)
                {
                    HtmlNode item = col[i];
                    string code = item.SelectSingleNode("./td[1]").InnerText;
                    string absent = Function.TidyText(item.SelectSingleNode("./td[3]").InnerText);
                    string firstMidMark = Function.TidyText(item.SelectSingleNode("./td[4]").InnerText);
                    string secondMidMark = Function.TidyText(item.SelectSingleNode("./td[5]").InnerText);
                    string thirdMidMark = Function.TidyText(item.SelectSingleNode("./td[6]").InnerText);
                    string finalMark = Function.TidyText(item.SelectSingleNode("./td[7]").InnerText);
                    string intMark = Function.TidyText(item.SelectSingleNode("./td[8]").InnerText);
                    string average = Function.TidyText(item.SelectSingleNode("./td[9]").InnerText);
                    string grade = Function.TidyText(item.SelectSingleNode("./td[10]").InnerText);
                    string state = Function.TidyText(item.SelectSingleNode("./td[11]").InnerText);
                    string firstMidDate = Function.TidyText(item.SelectSingleNode("./td[12]").InnerText);
                    string secondMidDate = Function.TidyText(item.SelectSingleNode("./td[13]").InnerText);
                    string thirdMidDate = Function.TidyText(item.SelectSingleNode("./td[14]").InnerText);
                    string finalDate = Function.TidyText(item.SelectSingleNode("./td[15]").InnerText);
                    string intDate = Function.TidyText(item.SelectSingleNode("./td[16]").InnerText);
                    string intRight = Function.TidyText(item.SelectSingleNode("./td[17]").InnerText);
                    Lesson lesson = currentPeriod.Lessons.SingleOrDefault(x => x.Code == code);
                    if (lesson != null)
                    {
                        lesson.AbsentState = absent == "G" ? AbsentType.Passed : absent == "D" ? AbsentType.Failed : absent == "M" ? AbsentType.Free : (AbsentType?)null;
                        if (!string.IsNullOrEmpty(firstMidMark))
                            lesson.FirstMidterm = new Exam() { Mark = Convert.ToInt32(firstMidMark), Date = !string.IsNullOrEmpty(firstMidDate) ? Convert.ToDateTime(firstMidDate) : (DateTime?)null };
                        if (!string.IsNullOrEmpty(secondMidMark))
                            lesson.FirstMidterm = new Exam() { Mark = Convert.ToInt32(secondMidMark), Date = !string.IsNullOrEmpty(secondMidDate) ? Convert.ToDateTime(secondMidDate) : (DateTime?)null };
                        if (!string.IsNullOrEmpty(thirdMidMark))
                            lesson.FirstMidterm = new Exam() { Mark = Convert.ToInt32(thirdMidMark), Date = !string.IsNullOrEmpty(thirdMidDate) ? Convert.ToDateTime(thirdMidDate) : (DateTime?)null };
                        if (!string.IsNullOrEmpty(finalMark))
                            lesson.FirstMidterm = new Exam() { Mark = Convert.ToInt32(finalMark), Date = !string.IsNullOrEmpty(finalDate) ? Convert.ToDateTime(finalDate) : (DateTime?)null };
                        if (!string.IsNullOrEmpty(intMark))
                            lesson.FirstMidterm = new Exam() { Mark = Convert.ToInt32(intMark), Date = !string.IsNullOrEmpty(intDate) ? Convert.ToDateTime(intDate) : (DateTime?)null };
                        if (!string.IsNullOrEmpty(average))
                            lesson.Average = Convert.ToInt32(average);
                        lesson.Grade = Lesson.GetGrade(grade);
                        lesson.State = state == "Geçti" ? true : state == "Kaldı" ? false : (bool?)null;
                        if (!string.IsNullOrEmpty(intRight))
                            lesson.IntegrationRight = intRight == "Yok" ? false : true;
                    }
                }
                this.State = true;
                this.Message = null;
                this.DataObject = currentPeriod;
            }
            catch (Exception)
            {
                this.State = false;
                this.Message = "Geçerli döneme erişilemiyor..";
                this.DataObject = null;
            }
            return this;
        }
        public Connection GetFoodLists()
        {
            try
            {
                EruWebServisClient client = new EruWebServisClient();
                ResponseObject response = client.YemekList();
                List<YemekModel> list = ((YemekModel[])response.DataObject).ToList();
                List<FoodList> FoodLists = new List<FoodList>();
                foreach (var food in list)
                {
                    FoodList foodList = new FoodList();
                    foodList.Date = Convert.ToDateTime(food.date.Split(' ')[0].Replace("101", "01").Replace("2013", "2014"));
                    foodList.Foods = new List<Food>();
                    string[] split = food.YemekLunch.Replace("\n", "").Replace("<br>", "").Split('\r');
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i] != "")
                        {
                            string[] kaloriSplit = split[i].Split('(');
                            string kalori = kaloriSplit[kaloriSplit.Length - 1];
                            foodList.Foods.Add(new Food() { Name = Function.TidyText(split[i].Replace("(" + kalori, "")), Calorie = kalori.ToLower().Replace("kcal)", "").Trim() });
                        }
                    }
                    FoodLists.Add(foodList);
                }
                this.State = true;
                this.Message = null;
                this.DataObject = new ListModel() { List = FoodLists.OrderBy(x => x.Date).ToArray() };
            }
            catch (Exception)
            {
                this.State = false;
                this.Message = "Yemek listesine erişilemiyor..";
                this.DataObject = null;
            }
            return this;
        }
        public Connection GetActivities()
        {
            try
            {
                EruWebServisClient client = new EruWebServisClient();
                ResponseObject response = client.CalenderAndDetail(DateTime.Now);
                List<CalenderModel> list = ((CalenderModel[])response.DataObject).ToList();
                List<Activity> Activities = new List<Activity>();
                foreach (var calender in list)
                {
                    for (int i = 0; i < calender.NCalDetail.Length; i++)
                    {
                        Activity activity = new Activity();
                        activity.Date = calender.tarih;
                        activity.Title = calender.NCalDetail[i].event_title.Trim();
                        if (!string.IsNullOrEmpty(activity.Title))
                            Activities.Add(activity);
                    }
                }
                this.State = true;
                this.Message = null;
                this.DataObject = new ListModel() { List = Activities.OrderBy(x => x.Date).ToArray() };
            }
            catch (Exception)
            {
                this.State = false;
                this.Message = "Etkinliklere erişilemiyor..";
                this.DataObject = null;
            }
            return this;
        }

    }
}