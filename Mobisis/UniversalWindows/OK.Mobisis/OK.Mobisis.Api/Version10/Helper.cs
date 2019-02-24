using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using OK.Mobisis.Entities.ApiEntities;
using OK.Mobisis.Entities.Enums;
using OK.Mobisis.Entities.ModelEntities;
using System.Xml.Linq;

namespace OK.Mobisis.Api.Version10
{
    public static class Helper
    {
        public static string DefaultURL { get { return "http://obisis.erciyes.edu.tr"; } }
        public static string GraduationURL { get; set; }
        public static string LessonStateURL { get; set; }
        public static string DocumentsURL { get; set; }
        public static string LessonsURL { get; set; }
        public static string FoodListsURL { get { return "http://www.erciyes.edu.tr/tr/index.asp?menu=KampusteYasam&eru_sayfa=oyemek"; } }
        
        public static string GetHtml(URLs url, string studentId = null, string password = null, string additionalData = null)
        {
            try
            {
                string html, urlString;
                var cookieContainer = new CookieContainer();
                if (studentId == null || password == null)
                {
                    urlString = FoodListsURL;
                    var request = HttpWebRequest.Create(urlString) as HttpWebRequest;
                    var response = request.GetResponse() as HttpWebResponse;
                    var reader = new StreamReader(response.GetResponseStream());
                    html = reader.ReadToEnd();
                }
                else
                {
                    var request = HttpWebRequest.Create(DefaultURL) as HttpWebRequest;
                    var response = request.GetResponse() as HttpWebResponse;
                    var reader = new StreamReader(response.GetResponseStream());
                    html = reader.ReadToEnd();

                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var inputCollection = doc.DocumentNode.SelectNodes("//input");

                    string postData = string.Format("__VIEWSTATE={0}&__EVENTVALIDATION={1}&ctl02$txtboxOgrenciNo={2}&ctl02$txtBoxSifre={3}&ctl02$btnLogin={4}",
                                      HttpUtility.UrlEncode(inputCollection[1].Attributes["value"].Value),
                                      HttpUtility.UrlEncode(inputCollection[3].Attributes["value"].Value),
                                      HttpUtility.UrlEncode(studentId),
                                      HttpUtility.UrlEncode(password),
                                      HttpUtility.UrlEncode("Giriş"));

                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                    var loginRequest = HttpWebRequest.Create(DefaultURL) as HttpWebRequest;
                    loginRequest.Method = "POST";
                    loginRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                    loginRequest.CookieContainer = cookieContainer;
                    loginRequest.ContentLength = byteArray.Length;
                    loginRequest.KeepAlive = true;

                    var loginStream = loginRequest.GetRequestStream();
                    loginStream.Write(byteArray, 0, byteArray.Length);
                    loginStream.Close();

                    var loginResponse = loginRequest.GetResponse();

                    html = new StreamReader(loginResponse.GetResponseStream()).ReadToEnd();

                    SetURLs(html);

                    switch (url)
                    {
                        case URLs.DefaultURL:
                            urlString = DefaultURL;
                            break;
                        case URLs.GraduationURL:
                            urlString = GraduationURL;
                            break;
                        case URLs.LessonStateURL:
                            urlString = LessonStateURL;
                            break;
                        case URLs.DocumentsURL:
                            urlString = DocumentsURL;
                            break;
                        case URLs.LessonsURL:
                            urlString = LessonsURL;
                            break;
                        case URLs.FoodListsURL:
                            urlString = FoodListsURL;
                            break;
                        default:
                            urlString = DefaultURL;
                            break;
                    }

                    if (!string.IsNullOrEmpty(additionalData))
                    {
                        var req = WebRequest.Create(urlString) as HttpWebRequest;
                        req.CookieContainer = cookieContainer;
                        var res = req.GetResponse() as HttpWebResponse;
                        var stream = new StreamReader(res.GetResponseStream());
                        html = stream.ReadToEnd();

                        doc.LoadHtml(html);
                        inputCollection = doc.DocumentNode.SelectNodes("//input");

                        string _postData = string.Format("__VIEWSTATE={0}&__EVENTVALIDATION={1}&__LASTFOCUS=&__EVENTARGUMENT=&__VIEWSTATEGENERATOR={2}" + additionalData,
                                      HttpUtility.UrlEncode(inputCollection[1].Attributes["value"].Value),
                                      HttpUtility.UrlEncode(inputCollection[3].Attributes["value"].Value),
                                      HttpUtility.UrlEncode(inputCollection[2].Attributes["value"].Value));

                        byte[] additionalByteArray = Encoding.UTF8.GetBytes(_postData);

                        var readRequest = WebRequest.Create(urlString) as HttpWebRequest;
                        readRequest.Method = "POST";
                        readRequest.ContentType = "application/x-www-form-urlencoded";
                        readRequest.UserAgent = "runscope/0.1,Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36";
                        readRequest.Headers.Add("Cache-Control", "max-age=0");
                        readRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                        readRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                        readRequest.Headers.Add("Upgrade", "1");
                        readRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                        readRequest.Referer = urlString;
                        readRequest.CookieContainer = cookieContainer;
                        readRequest.ContentLength = additionalByteArray.Length;
                        readRequest.KeepAlive = true;

                        var readStream = readRequest.GetRequestStream();
                        readStream.Write(additionalByteArray, 0, additionalByteArray.Length);
                        readStream.Close();

                        var readResponse = readRequest.GetResponse() as HttpWebResponse;
                        var readReponseStream = new StreamReader(readResponse.GetResponseStream());
                        html = readReponseStream.ReadToEnd();
                    }
                    else
                    {
                        var readRequest = WebRequest.Create(urlString) as HttpWebRequest;
                        readRequest.CookieContainer = cookieContainer;
                        var readResponse = readRequest.GetResponse() as HttpWebResponse;
                        var readStream = new StreamReader(readResponse.GetResponseStream());
                        html = readStream.ReadToEnd();
                    }


                }
                return html;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetHtml(string urlString, string studentId, string password)
        {
            string html;
            var cookieContainer = new CookieContainer();
            var request = HttpWebRequest.Create(DefaultURL) as HttpWebRequest;
            var response = request.GetResponse() as HttpWebResponse;
            var reader = new StreamReader(response.GetResponseStream());
            html = reader.ReadToEnd();

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var inputCollection = doc.DocumentNode.SelectNodes("//input");

            string postData = string.Format("__VIEWSTATE={0}&__EVENTVALIDATION={1}&ctl02$txtboxOgrenciNo={2}&ctl02$txtBoxSifre={3}&ctl02$btnLogin={4}",
                              HttpUtility.UrlEncode(inputCollection[1].Attributes["value"].Value),
                              HttpUtility.UrlEncode(inputCollection[3].Attributes["value"].Value),
                              HttpUtility.UrlEncode(studentId),
                              HttpUtility.UrlEncode(password),
                              HttpUtility.UrlEncode("Giriş"));

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            var loginRequest = HttpWebRequest.Create(DefaultURL) as HttpWebRequest;
            loginRequest.Method = "POST";
            loginRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            loginRequest.CookieContainer = cookieContainer;
            loginRequest.ContentLength = byteArray.Length;
            loginRequest.KeepAlive = true;

            var loginStream = loginRequest.GetRequestStream();
            loginStream.Write(byteArray, 0, byteArray.Length);
            loginStream.Close();

            var loginResponse = loginRequest.GetResponse();

            html = new StreamReader(loginResponse.GetResponseStream()).ReadToEnd();

            var readRequest = WebRequest.Create(urlString) as HttpWebRequest;
            readRequest.CookieContainer = cookieContainer;
            var readResponse = readRequest.GetResponse() as HttpWebResponse;
            var readStream = new StreamReader(readResponse.GetResponseStream());
            html = readStream.ReadToEnd();
            return html;
        }

        private static void SetURLs(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode;
            LessonsURL = HttpUtility.HtmlDecode("http://obisis.erciyes.edu.tr/" + node.SelectSingleNode("//a[contains(text(), 'Not Sorgu')]").Attributes["href"].Value);
            GraduationURL = HttpUtility.HtmlDecode("http://obisis.erciyes.edu.tr/" + node.SelectSingleNode("//a[contains(text(), 'Mezuniyet')]").Attributes["href"].Value);
            DocumentsURL = HttpUtility.HtmlDecode("http://obisis.erciyes.edu.tr/" + node.SelectSingleNode("//a[contains(text(), 'Belge İstek')]").Attributes["href"].Value);
            LessonStateURL = HttpUtility.HtmlDecode("http://obisis.erciyes.edu.tr/" + node.SelectSingleNode("//a[contains(text(), 'Ders Durum')]").Attributes["href"].Value);
        }

        private static string GetFoodListString()
        {
            var request = HttpWebRequest.Create("http://mobil.erciyes.edu.tr/EruServiceWeb/EruWebServis.svc") as HttpWebRequest;
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", "http://tempuri.org/IEruWebServis/YemekList");
            request.Method = "POST";

            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"><soap:Body>");
            sb.Append("</soap:Body></soap:Envelope>");

            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());

            using (Stream readStream = request.GetRequestStream())
            {
                readStream.Write(byteArray, 0, byteArray.Length);
                readStream.Close();
            }

            string html = "";
            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    html = new StreamReader(stream).ReadToEnd();
                }
            }
            return html.Replace("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">", "").Replace("<s:Body>", "").Replace("</s:Body>", "").Replace("</s:Envelope>", "");
        

        }

        public static List<FoodList> GetFoodLists()
        {
            var foodLists = new List<FoodList>();
            string xml = GetFoodListString().Replace("a:", "");

            XElement root = XElement.Parse(xml);
            root = root.Elements().FirstOrDefault();
            root = root.Elements().FirstOrDefault();
            var items = root.Elements();

            foreach (var item in items)
            {
                var foodList = new FoodList();
                foodList.Foods = new List<Food>();
                string[] foods = item.Elements().ToList()[0].Value.Replace("<br>", "$").Split('$');
                foreach (var food in foods)
                {
                    foodList.Foods.Add(new Food() { Name = food.Tidy() });
                }
                foodList.Date = Convert.ToDateTime(item.Elements().ToList()[2].Value);
                foodLists.Add(foodList);
            }
            return foodLists;
        }

        public static List<FoodList> OldGetFoodLists()
        {
            string html = GetHtml(URLs.FoodListsURL);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode;

            var mnde = node.SelectSingleNode("//center");
            var tds = mnde.SelectNodes("//td").Where(x => x.Attributes["width"] != null && x.Attributes["width"].Value == "50%" && x.InnerText != "" && x.InnerText != "&nbsp;").ToList();

            string tempDate1 = "";
            string tempDate2 = "";

            var lists = new List<FoodList>();

            for (int i = 0; i < tds.Count(); i++)
            {
                var nd = tds[i] as HtmlNode;
                if (i % 4 == 0)
                {
                    tempDate1 = new string(nd.InnerText.ToCharArray().Where(c => char.IsDigit(c) || c == '.').ToArray());
                }
                else if (i % 4 == 1)
                {
                    tempDate2 = new string(nd.InnerText.ToCharArray().Where(c => char.IsDigit(c) || c == '.').ToArray());
                }
                else
                {
                    var list = new FoodList();
                    list.Date = Convert.ToDateTime((i % 4 == 2 ? tempDate1 : tempDate2));
                    list.Foods = new List<Food>();

                    var split = nd.InnerText.Replace("\t", "").Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < split.Length; j++)
                    {
                        string foodName = split[j].Trim();
                        if (!string.IsNullOrEmpty(foodName))
                        {
                            if (foodName.Contains("AKŞAM"))
                                break;

                            if (foodName.Contains("ÖĞLE"))
                                foodName = foodName.Replace("ÖĞLE", "");

                            foodName = foodName.Tidy();
                            if (!string.IsNullOrEmpty(foodName) && foodName != "T A T İ L" && foodName.ToUpper() != "TATİL")
                                list.Foods.Add(new Food() { Name = foodName });
                        }
                    }
                    lists.Add(list);
                }
            }

            return lists.OrderBy(x => x.Date).ToList();
        }

        internal static AbsentType? GetAbsentType(string type)
        {
            switch (type)
            {
                case "M":
                    return AbsentType.Free;
                case "G":
                    return AbsentType.Passed;
                case "D":
                    return AbsentType.Failed;
                default:
                    return null;
            }
        }

        internal static GradeType? GetGradeType(string grade)
        {
            switch (grade)
            {
                case "FF":
                    return GradeType.FF;
                case "FD":
                    return GradeType.FD;
                case "DD":
                    return GradeType.DD;
                case "DC":
                    return GradeType.DC;
                case "CC":
                    return GradeType.CC;
                case "CB":
                    return GradeType.CB;
                case "BB":
                    return GradeType.BB;
                case "BA":
                    return GradeType.BA;
                case "AA":
                    return GradeType.AA;
                case "ST":
                    return GradeType.ST;
                case "GT":
                    return GradeType.GT;
                default:
                    return null;
            }
        }

        internal static LessonState? GetLessonState(string state)
        {
            switch (state)
            {
                case "Geçti":
                    return LessonState.Passed;
                case "Kaldı":
                    return LessonState.Failed;
                case "Devamsız":
                    return LessonState.Absent;
                default:
                    return null;
            }
        }
        
        internal static Student GetStudent(StudentObject studentObject)
        {
            var student = new Student();
            student.Id = studentObject.Id;
            student.Password = studentObject.Password;
            
            student.Programs = student.GetPrograms();
            student.Graduation = student.GetGraduation();
            student.Documents = student.GetDocuments();
            
            return student;
        }

        internal static List<Program> GetPrograms(StudentObject studentObject)
        {
            var student = new Student();
            student.Id = studentObject.Id;
            student.Password = studentObject.Password;

            return student.GetPrograms();
        }

        internal static List<Document> GetDocuments(StudentObject studentObject)
        {
            var student = new Student();
            student.Id = studentObject.Id;
            student.Password = studentObject.Password;

            return student.GetDocuments();
        }

        internal static List<Graduation> GetGraduation(StudentObject studentObject)
        {
            var student = new Student();
            student.Id = studentObject.Id;
            student.Password = studentObject.Password;

            return student.GetGraduation();
        }

        internal static List<Period> GetPeriods(ProgramObject programObject)
        {
            var student = new Student();
            student.Id = programObject.StudentId;
            student.Password = programObject.Password;

            var program = new Program();
            program.Code = programObject.Code;

            return program.GetPeriods(student);
        }

        internal static List<Lesson> GetLessons(PeriodObject periodObject)
        {
            var student = new Student();
            student.Id = periodObject.StudentId;
            student.Password = periodObject.Password;

            /*
            var period = new Period();
            period.Code = periodObject.Code;
            period.YearCode = periodObject.YearCode;
            period.ProgramCode = periodObject.ProgramCode == 0 ? 1 : periodObject.ProgramCode;

            return period.GetLessons(student);*/

            var programObject = new ProgramObject()
            {
                StudentId = periodObject.StudentId,
                Password = periodObject.Password,
                Code = periodObject.ProgramCode
            };
            if (periodObject.ProgramCode == 0)
                programObject.Code = 1;
            var periods = GetPeriods(programObject);
            var period = periods.FirstOrDefault(x => x.Code == periodObject.Code && x.YearCode == periodObject.YearCode);
            var lessons = period.Lessons;
            /*if (programObject.StudentId == "1030515866")
            {
                lessons.ForEach(x => {
                    x.FirstMidterm = new Exam() { Mark = (new Random().Next() % 100) + 1, Date = DateTime.Now };
                });
            }*/
            return lessons;
        }

        internal static List<FriendMark> GetMarkList(LessonObject lessonObject)
        {
            var student = new Student();
            student.Id = lessonObject.StudentId;
            student.Password = lessonObject.Password;

            var lesson = new Lesson();
            lesson.Hash = lessonObject.Hash;

            return lesson.GetMarkList(student);
        }

        #region newUpdate
        public static string GetString(string action, Dictionary<string, string> parameters)
        {
            var request = HttpWebRequest.Create("http://obisis.erciyes.edu.tr/MobileWebService/Service1.svc") as HttpWebRequest;
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", "http://tempuri.org/IService1/" + action);
            request.Method = "POST";

            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"><soap:Body>");
            sb.Append("<" + action + " xmlns=\"http://tempuri.org/\">");
            foreach (var item in parameters)
            {
                sb.Append("<" + item.Key + ">" + item.Value + "</" + item.Key + ">");
            }
            sb.Append("</" + action + ">");
            sb.Append("</soap:Body></soap:Envelope>");

            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());

            using (Stream readStream = request.GetRequestStream())
            {
                readStream.Write(byteArray, 0, byteArray.Length);
                readStream.Close();
            }

            string html = "";
            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    html = new StreamReader(stream).ReadToEnd();
                }
            }
            return html.Replace("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">", "").Replace("<s:Body>", "").Replace("</s:Body>", "").Replace("</s:Envelope>", "");
        }

        internal static Student getStudentt(StudentObject studentObject)
        {
            var student = new Student();
            student.Id = studentObject.Id;
            student.Password = studentObject.Password;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("StudentNo", student.Id);
            parameters.Add("pass", student.Password);
            string html = GetString("StudentLogin", parameters).Replace("a:", "");
            XElement root = XElement.Parse(html);
            root = root.Elements().FirstOrDefault();
            root = root.Elements().FirstOrDefault();
            student.Name = root.Elements().ToList()[0].Value.Tidy() + " " + root.Elements().ToList()[6].Value.Tidy();
            
            student.Programs = getProgramss(student);

            student.Graduation = student.GetGraduation();
            student.Documents = student.GetDocuments();
            return student;
        }

        private static List<Period> getPeriodss(StudentObject studentObject, int programCode)
        {
            var periods = new List<Period>();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("StudentNo", studentObject.Id);
            parameters.Add("pass", studentObject.Password);
            string html = GetString("Filtering", parameters).Replace("a:", "");

            XElement root = XElement.Parse(html);
            root = root.Elements().FirstOrDefault();
            root = root.Elements().FirstOrDefault();
                                    
            foreach (var yil in root.Elements().ToList()[3].Elements())
            {
                foreach (var donem in root.Elements().ToList()[1].Elements())
                {
                    string periodName = donem.Elements().ToList()[0].Value.Tidy();
                    int periodCode = Convert.ToInt32(donem.Elements().ToList()[1].Value);
                    string year = yil.Elements().ToList()[0].Elements().ToList()[0].Value;
                    int yearCode = Convert.ToInt32(yil.Elements().ToList()[0].Elements().ToList()[1].Value);

                    var period = new Period();
                    period.Code = periodCode;
                    period.Name = periodName;
                    period.Year = year;
                    period.YearCode = yearCode;
                    period.ProgramCode = programCode;
                    period.Lessons = getLessonss(studentObject, programCode, yearCode, periodCode);
                    
                    if (period.Lessons.Any())
                        periods.Add(period);
                }
            }

            return periods;
        }

        private static List<Lesson> getLessonss(StudentObject studentObject, int programCode, int yearCode, int periodCode)
        {
            var lessons = new List<Lesson>();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("StudentNo", studentObject.Id);
            parameters.Add("yearCode", "" + yearCode);
            parameters.Add("yearCodeSpecified", "true");
            parameters.Add("donemNo", "" + periodCode);
            parameters.Add("donemNoSpecified", "true");
            parameters.Add("dalId", "" + programCode);
            parameters.Add("dalIdSpecified", "true");
            parameters.Add("pass", studentObject.Password);
            string html = GetString("NotSorgu", parameters).Replace("a:", "");

            XElement root = XElement.Parse(html);
            root = root.Elements().FirstOrDefault();
            root = root.Elements().FirstOrDefault();

            foreach (var item in root.Elements().Where(x => x.HasElements))
            {
                Lesson lesson = new Lesson();

                lesson.Code = item.Elements().ToList()[6].Value;
                lesson.AbsentState = GetAbsentType(item.Elements().ToList()[7].Value);
                lesson.Name = item.Elements().ToList()[5].Value.Tidy();
                lesson.FirstMidterm = new Exam()
                {
                    Mark = string.IsNullOrEmpty(item.Elements().ToList()[16].Value) ? (int?)null : item.Elements().ToList()[16].Value == "G" ? 0 : Convert.ToInt32(item.Elements().ToList()[16].Value),
                    Date = string.IsNullOrEmpty(item.Elements().ToList()[18].Value) ? (DateTime?)null : Convert.ToDateTime(item.Elements().ToList()[18].Value)
                };
                lesson.SecondMidterm = new Exam()
                {
                    Mark = string.IsNullOrEmpty(item.Elements().ToList()[19].Value) ? (int?)null : item.Elements().ToList()[19].Value == "G" ? 0 : Convert.ToInt32(item.Elements().ToList()[19].Value),
                    Date = string.IsNullOrEmpty(item.Elements().ToList()[21].Value) ? (DateTime?)null : Convert.ToDateTime(item.Elements().ToList()[21].Value)
                };
                lesson.ThirdMidterm = new Exam()
                {
                    Mark = string.IsNullOrEmpty(item.Elements().ToList()[22].Value) ? (int?)null : item.Elements().ToList()[22].Value == "G" ? 0 : Convert.ToInt32(item.Elements().ToList()[22].Value),
                    Date = string.IsNullOrEmpty(item.Elements().ToList()[24].Value) ? (DateTime?)null : Convert.ToDateTime(item.Elements().ToList()[24].Value)
                };
                lesson.Final = new Exam()
                {
                    Mark = string.IsNullOrEmpty(item.Elements().ToList()[10].Value) ? (int?)null : item.Elements().ToList()[10].Value == "G" ? 0 : Convert.ToInt32(item.Elements().ToList()[10].Value),
                    Date = string.IsNullOrEmpty(item.Elements().ToList()[12].Value) ? (DateTime?)null : Convert.ToDateTime(item.Elements().ToList()[12].Value)
                };
                lesson.Integration = new Exam()
                {
                    Mark = string.IsNullOrEmpty(item.Elements().ToList()[0].Value) ? (int?)null : item.Elements().ToList()[0].Value == "G" ? 0 : Convert.ToInt32(item.Elements().ToList()[0].Value),
                    Date = string.IsNullOrEmpty(item.Elements().ToList()[4].Value) ? (DateTime?)null : Convert.ToDateTime(item.Elements().ToList()[4].Value)
                };
                lesson.IntegrationRight = item.Elements().ToList()[2].Value == "true";
                lessons.Add(lesson);
            }
            return lessons;
        }

        public static List<Program> getProgramss(Student student)
        {
            var programs = new List<Program>();
            string html = Helper.GetHtml(URLs.LessonsURL, student.Id, student.Password);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode;

            student.Name = node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtAdiSoyadi']").InnerText.Tidy();
            var root = node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtAdiSoyadi']").ParentNode.ParentNode.ParentNode;

            var programNodes = root.SelectNodes("//tr[2]//td[3]//table[1]//tr[2]//td[2]//table[1]//tr[1]//td");
            for (int i = 0; i < programNodes.Count(); i++)
            {
                var programNode = programNodes[i];
                if (!string.IsNullOrEmpty(programNode.InnerText))
                {
                    var program = new Program();
                    program.Code = (i + 1);
                    program.Name = programNode.InnerText.Tidy();

                    if (programNode.InnerText.Trim() == "ANADAL")
                        program.Type = ProgramType.Major;
                    else if (programNode.InnerText.Trim() == "İKİNCİ ANADAL")
                        program.Type = ProgramType.SecondMajor;
                    else
                        program.Type = ProgramType.Minor;

                    program.Periods = getPeriodss(new StudentObject() { Id = student.Id, Password = student.Password }, program.Code);
                    programs.Add(program);
                }
            }

            var facultyNodes = root.SelectNodes("//tr[2]//td[3]//table[1]//tr[2]//td[2]//table[1]//tr[2]//td");
            for (int i = 0; i < facultyNodes.Count(); i++)
            {
                var facultyNode = facultyNodes[i];
                if (!string.IsNullOrEmpty(facultyNode.InnerText))
                {
                    programs[i].Faculty = facultyNode.InnerText.Tidy();
                }
            }

            var departmentNodes = root.SelectNodes("//tr[2]//td[3]//table[1]//tr[2]//td[2]//table[1]//tr[3]//td");
            for (int i = 0; i < departmentNodes.Count(); i++)
            {
                var departmentNode = departmentNodes[i];
                if (!string.IsNullOrEmpty(departmentNode.InnerText))
                {
                    programs[i].Department = departmentNode.InnerText.Tidy();
                }
            }

            var classNodes = root.SelectNodes("//tr[2]//td[3]//table[1]//tr[2]//td[2]//table[1]//tr[4]//td");
            for (int i = 0; i < classNodes.Count(); i++)
            {
                var classNode = classNodes[i];
                if (!string.IsNullOrEmpty(classNode.InnerText))
                {
                    if (classNode.InnerText.Contains("MEZUN"))
                        programs[i].Class = -1;
                    else if (classNode.InnerText.Contains("HAZIRLIK"))
                        programs[i].Class = 0;
                    else if (classNode.InnerText.Contains("("))
                        programs[i].Class = Convert.ToInt32(classNode.InnerText.Split('(')[1].Split('.')[0]);
                    else
                        programs[i].Class = -1;

                    if (classNode.InnerText.Contains("HAZIRLIK"))
                        programs[i].GANO = 0;
                    else
                        programs[i].GANO = Convert.ToDouble(classNode.InnerText.Split(':')[1].Replace(",", ".").Trim()) / 100;
                }
            }
            
            return programs;
        }
        #endregion
    }
}