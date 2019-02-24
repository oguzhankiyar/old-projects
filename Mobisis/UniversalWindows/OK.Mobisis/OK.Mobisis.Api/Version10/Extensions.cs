using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using OK.Mobisis.Api.ObisisMobileService;
using OK.Mobisis.Entities.Enums;
using OK.Mobisis.Entities.ModelEntities;

namespace OK.Mobisis.Api.Version10
{
    public static class Extensions
    {
        public static List<Program> GetPrograms(this Student student)
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

                    program.Periods = program.GetPeriods(student);
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
                if (!string.IsNullOrEmpty(classNode.InnerText.Replace("\n", "").Replace("\r", "").Trim()))
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
            
            for (int i = 0; i < programs.Count(); i++)
            {
                if (programs.Count() <= i)
                    break;

                var program = programs[i];
                if (string.IsNullOrEmpty(program.Faculty) || string.IsNullOrEmpty(program.Department))
                    programs.RemoveAll(x => x.Code == program.Code);
            }

            return programs;
        }

        public static List<Period> GetPeriods(this Program program, Student student)
        {
            var periods = new List<Period>();
            var doc = new HtmlDocument();
            string html = Helper.GetHtml(URLs.LessonStateURL, student.Id, student.Password, "__EVENTTARGET=ctl02%24ddlNcy&ctl02%24ddlNcy=" + program.Code + "");
            doc.LoadHtml(html);
            var node = doc.DocumentNode;
            var col = node.SelectNodes("//table[@id='ctl02_gvDersDurum']//tr");
            var period = new Period();
            if (col != null)
            {
                for (int i = 1; i < col.Count; i++)
                {
                    var item = col[i];
                    if (item.InnerText.Contains("ORTALAMASI"))
                    {
                        if (i != 1)
                            periods.Add(period);

                        period = new Period();
                        string text = item.InnerText.Replace("\r", "").Replace("\t", "").Replace("\n", "");
                        period.Name = text.Split(' ')[1].Tidy();
                        period.Code = period.GetCode();
                        period.Year = text.Split(' ')[0];
                        period.YearCode = period.GetYearCode();
                        period.ProgramCode = program.Code;
                        period.GANO = Convert.ToDouble(text.Split(':')[1].Replace(",", "").Replace(".", "").Trim()) / 100;
                        //period.Lessons = period.GetLessons(student);
                        period.Lessons = period.GetLessons(student);
                    }
                    else
                    {
                        var lesson = period.Lessons.FirstOrDefault(x => x.Name == item.SelectSingleNode("./td[2]").InnerText.Tidy());
                        if (lesson == null)
                            lesson = new Lesson();
                        lesson.Code = item.SelectSingleNode("./td[1]").InnerText;
                        lesson.Name = item.SelectSingleNode("./td[2]").InnerText.Tidy();
                        lesson.Credit = Convert.ToDouble(item.SelectSingleNode("./td[6]").InnerText.Replace(",", "").Replace(".", "").Trim()) / 100;
                        lesson.AverageEffect = item.SelectSingleNode("./td[8]").InnerText != "&nbsp;" && item.SelectSingleNode("./td[9]").InnerText == "&nbsp;" ? false : true;
                        lesson.SuccessFactor = item.SelectSingleNode("./td[9]").InnerText == "&nbsp;" ? 0.0 : Convert.ToDouble(item.SelectSingleNode("./td[9]").InnerText.Trim());
                        lesson.Class = Convert.ToInt32(item.SelectSingleNode("./td[3]").InnerText);
                    }
                    if (i == col.Count - 1)
                        periods.Add(period);
                }
            }
            return periods.OrderBy(x => x.YearCode).ThenBy(x => x.Code).ToList();
        }

        public static List<Lesson> GetLessons(this Period period, Student student)
        {/*
            var lessons = new List<Lesson>();
            var client = new ObisisMobileServiceClient();
            var studentResult = client.OgrenciBilgiGetir(student.Id, student.Password, period.YearCode, period.Code);
            var dt = studentResult.Data.Tables[0];

            foreach (DataRow data in dt.Rows)
            {
                var lesson = new Lesson();
                lesson.Name = data[0].ToString().Tidy();
                lesson.FirstMidterm = new Exam() { Mark = string.IsNullOrEmpty(data["VIZE1"].ToString()) ? (int?)null : Convert.ToInt32(data["VIZE1"]) };
                lesson.SecondMidterm = new Exam() { Mark = string.IsNullOrEmpty(data["VIZE2"].ToString()) ? (int?)null : Convert.ToInt32(data["VIZE2"]) };
                lesson.ThirdMidterm = new Exam() { Mark = string.IsNullOrEmpty(data["VIZE3"].ToString()) ? (int?)null : Convert.ToInt32(data["VIZE3"]) };
                lesson.Final = new Exam() { Mark = string.IsNullOrEmpty(data["FINAL"].ToString()) ? (int?)null : Convert.ToInt32(data["FINAL"]) };
                lesson.Integration = new Exam() { Mark = string.IsNullOrEmpty(data["BUTUNLEME"].ToString()) ? (int?)null : Convert.ToInt32(data["BUTUNLEME"]) };
                lesson.Average = string.IsNullOrEmpty(data["ORTALAMA"].ToString()) ? (int?)null : Convert.ToInt32(data["ORTALAMA"]);
                lesson.Grade = string.IsNullOrEmpty(data["HARF_NOTU"].ToString()) ? null : Helper.GetGradeType(data["HARF_NOTU"].ToString());
                lesson.State = string.IsNullOrEmpty(data["GECTI_KALDI"].ToString()) ? (LessonState?)null : Helper.GetLessonState(data["GECTI_KALDI"].ToString().Tidy());
                lessons.Add(lesson);
            }

            return lessons;*/

            var lessons = new List<Lesson>();
            var doc = new HtmlDocument();
            string html = Helper.GetHtml(URLs.LessonsURL, student.Id, student.Password, "__EVENTTARGET=ctl02%24dlNcy&ctl02%24dlOgretimYillari=" + period.YearCode + "&ctl02%24dlDonem=" + period.Code + "&ctl02%24dlNcy=" + period.ProgramCode + "");
            doc.LoadHtml(html);

            var node = doc.DocumentNode;
            var col = node.SelectNodes("//table[@id='ctl02_dgDers']//tr");
            if (col == null)
                return lessons;
            for (int i = 1; i < col.Count; i++)
            {
                var item = col[i] as HtmlNode;
                var tdCol = item.SelectNodes("./td");

                var lesson = new Lesson();
                lesson.Code = tdCol[0].InnerText;
                lesson.Name = tdCol[1].InnerText.Tidy();
                lesson.AbsentState = Helper.GetAbsentType(tdCol[3].InnerText);
                lesson.FirstMidterm = new Exam() { Mark = tdCol[4].InnerText == "&nbsp;" ? (int?)null : tdCol[4].InnerText == "G" ? 0 : Convert.ToInt32(tdCol[4].InnerText), Date = tdCol[12].InnerText != "&nbsp;" ? Convert.ToDateTime(tdCol[12].InnerText) : (DateTime?)null };
                lesson.SecondMidterm = new Exam() { Mark = tdCol[5].InnerText == "&nbsp;" ? (int?)null : tdCol[5].InnerText == "G" ? 0 : Convert.ToInt32(tdCol[5].InnerText), Date = tdCol[13].InnerText != "&nbsp;" ? Convert.ToDateTime(tdCol[13].InnerText) : (DateTime?)null };
                lesson.ThirdMidterm = new Exam() { Mark = tdCol[6].InnerText == "&nbsp;" ? (int?)null : tdCol[6].InnerText == "G" ? 0 : Convert.ToInt32(tdCol[6].InnerText), Date = tdCol[14].InnerText != "&nbsp;" ? Convert.ToDateTime(tdCol[14].InnerText) : (DateTime?)null };
                lesson.Final = new Exam() { Mark = tdCol[7].InnerText == "&nbsp;" ? (int?)null : tdCol[7].InnerText == "G" ? 0 : Convert.ToInt32(tdCol[7].InnerText), Date = tdCol[15].InnerText != "&nbsp;" ? Convert.ToDateTime(tdCol[15].InnerText) : (DateTime?)null };
                lesson.Integration = new Exam() { Mark = tdCol[8].InnerText == "&nbsp;" ? (int?)null : tdCol[8].InnerText == "G" ? 0 : Convert.ToInt32(tdCol[8].InnerText), Date = tdCol[16].InnerText != "&nbsp;" ? Convert.ToDateTime(tdCol[16].InnerText) : (DateTime?)null };
                lesson.Average = tdCol[9].InnerText != "&nbsp;" ? Convert.ToInt32(tdCol[9].InnerText) : (int?)null;
                lesson.Grade = tdCol[10].InnerText != "&nbsp;" ? Helper.GetGradeType(tdCol[10].InnerText) : (GradeType?)null;
                lesson.State = tdCol[11].InnerText != "&nbsp;" ? Helper.GetLessonState(tdCol[11].InnerText) : (LessonState?)null;
                lesson.IntegrationRight = tdCol[17].InnerText != "&nbsp;" ? (bool?)null : (tdCol[17].InnerText == "Yok" ? false : true);
                lesson.Hash = tdCol[19].Descendants().Single(x => x.Name == "a").Attributes["href"].Value.Split('?')[1];
                lessons.Add(lesson);
            }
            return lessons;
        }

        public static List<Graduation> GetGraduation(this Student student)
        {
            var list = new List<Graduation>();
            var doc = new HtmlDocument();
            string html = Helper.GetHtml(URLs.GraduationURL, student.Id, student.Password);
            doc.LoadHtml(html);

            var node = doc.DocumentNode;
            var col = node.SelectNodes("//table[@id='ctl02_dgMezuniyetDurum']//tr");
            for (int i = 1; i < col.Count; i++)
            {
                var item = col[i];
                if (!string.IsNullOrEmpty(item.InnerText.Replace("&nbsp;", "").Trim()) && !item.InnerText.Contains("SONUÇ"))
                {
                    var grade = new Graduation();
                    grade.Condition = item.SelectSingleNode("./td[1]").InnerText.Tidy();
                    grade.State = item.SelectSingleNode("./td[2]").InnerText.Tidy();
                    grade.Value = !item.SelectSingleNode("./td[3]").InnerHtml.Contains("InValid");
                    grade.Description = item.SelectSingleNode("./td[4]").InnerText.Tidy();
                    list.Add(grade);
                }
            }
            return list;
        }

        public static List<Document> GetDocuments(this Student student)
        {
            var documents = new List<Document>();
            var doc = new HtmlDocument();
            string html = Helper.GetHtml(URLs.DocumentsURL, student.Id, student.Password);
            doc.LoadHtml(html);
            var node = doc.DocumentNode;
            var col = node.SelectNodes("//table[@id='ctl02_dgBelge']//tr");
            for (int i = 1; i < col.Count; i++)
            {
                var item = col[i];
                var document = new Document();
                document.Program = item.SelectSingleNode("./td[1]").InnerText.Contains("ANADAL") ? ProgramType.Major : ProgramType.Minor;
                document.Type = item.SelectSingleNode("./td[2]").InnerText;
                document.RequestDate = Convert.ToDateTime(item.SelectSingleNode("./td[3]").InnerText);
                if (!string.IsNullOrEmpty(item.SelectSingleNode("./td[4]").InnerText.Replace("&nbsp;", "").Trim()))
                    document.RequestCount = Convert.ToInt32(item.SelectSingleNode("./td[4]").InnerText);
                document.IsPrinted = false;
                if (!string.IsNullOrEmpty(item.SelectSingleNode("./td[5]").InnerText.Tidy()))
                {
                    document.IsPrinted = true;
                    document.PrintDate = Convert.ToDateTime(item.SelectSingleNode("./td[5]").InnerText);
                    document.PrintCount = Convert.ToInt32(item.SelectSingleNode("./td[6]").InnerText);
                }
                documents.Add(document);
            }
            return documents;
        }

        public static List<FriendMark> GetMarkList(this Lesson lesson, Student student)
        {
            string html = Helper.GetHtml("http://obisis.erciyes.edu.tr/NotListe.aspx?" + lesson.Hash, student.Id, student.Password);

            return new List<FriendMark>();
        }

        public static string Tidy(this string text)
        {
            try
            {
                text = HttpUtility.HtmlDecode(text);
                text = text.Trim();
                text = text.Replace("&nbsp;", "");
                if (!string.IsNullOrEmpty(text))
                {
                    text = text.Replace("-", " - ").Replace("/", " / ").Replace("  ", "");
                    string[] split = text.Split(' ');
                    text = "";
                    for (int i = 0; i < split.Length; i++)
                    {
                        split[i] = split[i].ToUpper();
                        if (split[i] == "VE")
                            text += "ve ";
                        else if (split[i] == "II")
                            text += "II ";
                        else if (split[i] == "İÖ")
                            text += "İÖ ";
                        else if (split[i] == "(İÖ)")
                            text += "(İÖ) ";
                        else
                        {
                            char[] chars = split[i].ToCharArray();
                            split[i] = chars[0].ToString().ToUpper();
                            for (int j = 1; j < chars.Length; j++)
                            {
                                split[i] += chars[j].ToString().ToLower(CultureInfo.GetCultureInfo("tr-tr"));
                            }
                            text += split[i] + " ";
                        }
                    }
                }
            }
            catch (Exception) { }
            return text.Trim();
        }

        internal static int GetYearCode(this Period period)
        {
            return (1976 - Convert.ToInt32(period.Year.Split('-')[0]));
        }

        internal static int GetCode(this Period period)
        {
            switch (period.Name.ToUpper())
            {
                case "GÜZ":
                    return -10;
                case "GÜZ SONU EK DÖNEM":
                    return -15;
                case "BAHAR":
                    return -20;
                case "BAHAR SONU EK DÖNEM":
                    return -25;
                case "YAZ":
                    return -30;
                case "YAZ SONU EK DÖNEM":
                    return -35;
                default:
                    return 0;
            }
        }
    }
}