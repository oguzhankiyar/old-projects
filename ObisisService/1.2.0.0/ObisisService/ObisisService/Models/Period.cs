using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using ObisisService.ObisisWebService;
using HtmlAgilityPack;
using System.Data;

namespace ObisisService.Models
{
    [DataContract]
    public class Period : ObjectModel
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public int YearCode { get; set; }
        [DataMember]
        public string Year { set; get; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double GANO { get; set; }
        [DataMember]
        public List<Lesson> Lessons { get; set; }

        public static List<Period> GetPeriods(string html)
        {
            List<Period> periods = new List<Period>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            HtmlNodeCollection col = node.SelectNodes("//table[@id='ctl00_gvDersDurum']//tr");
            Period period = new Period();
            ObisisMobileServiceClient client = new ObisisMobileServiceClient();
            for (int i = 1; i < col.Count; i++)
            {
                HtmlNode item = col[i];
                if (item.InnerText.Contains("ORTALAMASI"))
                {
                    if (i != 1)
                        periods.Add(period);
                    period = new Period();
                    string text = item.InnerText.Replace("\r", "").Replace("\t", "").Replace("\n", "");
                    period.Name = Function.TidyText(text.Split(' ')[1]);
                    period.Code = GetCode(period.Name);
                    period.Year = text.Split(' ')[0];
                    period.YearCode = GetYearCode(period.Year);
                    period.GANO = Convert.ToDouble(text.Split(':')[1].Replace(",", "").Replace(".", "").Trim()) / 100;
                    period.Lessons = new List<Lesson>();
                    OgrenciResult result = client.OgrenciBilgiGetir(Data.StudentID.ToString(), Data.Password, period.YearCode, period.Code);
                    DataTable dt = result.Data.Tables[0];
                    foreach (DataRow data in dt.Rows)
                    {
                        Lesson lesson = new Lesson()
                        {
                            Name = Function.TidyText(data["DERS_ADI_TR"].ToString()),
                            FirstMidterm = new Exam() { Mark = string.IsNullOrEmpty(data["VIZE1"].ToString()) ? (int?)null : Convert.ToInt32(data["VIZE1"]) },
                            SecondMidterm = new Exam() { Mark = string.IsNullOrEmpty(data["VIZE2"].ToString()) ? (int?)null : Convert.ToInt32(data["VIZE2"]) },
                            ThirdMidterm = new Exam() { Mark = string.IsNullOrEmpty(data["VIZE3"].ToString()) ? (int?)null : Convert.ToInt32(data["VIZE3"]) },
                            Final = new Exam() { Mark = string.IsNullOrEmpty(data["FINAL"].ToString()) ? (int?)null : Convert.ToInt32(data["FINAL"]) },
                            Integration = new Exam() { Mark = string.IsNullOrEmpty(data["BUTUNLEME"].ToString()) ? (int?)null : Convert.ToInt32(data["BUTUNLEME"]) },
                            Average = string.IsNullOrEmpty(data["ORTALAMA"].ToString()) ? (int?)null : Convert.ToInt32(data["ORTALAMA"]),
                            Grade = string.IsNullOrEmpty(data["HARF_NOTU"].ToString()) ? null : Lesson.GetGrade(data["HARF_NOTU"].ToString()),
                            State = string.IsNullOrEmpty(data["GECTI_KALDI"].ToString()) ? (bool?)null : data["GECTI_KALDI"].ToString() == "GEÇTİ"
                        };
                        period.Lessons.Add(lesson);
                    }
                }
                else
                {
                    string code = item.SelectSingleNode("./td[1]").InnerText;
                    string name = Function.TidyText(item.SelectSingleNode("./td[2]").InnerText);
                    double credit = Convert.ToDouble(item.SelectSingleNode("./td[6]").InnerText.Trim().Replace(",", "."));
                    int cls = Convert.ToInt32(item.SelectSingleNode("./td[3]").InnerText);
                    bool ganoEffect = item.SelectSingleNode("./td[8]").InnerText != "&nbsp;" && item.SelectSingleNode("./td[9]").InnerText == "&nbsp;" ? false : true;
                    double factor = item.SelectSingleNode("./td[9]").InnerText == "&nbsp;" ? 0.0 : Convert.ToDouble(item.SelectSingleNode("./td[9]").InnerText.Trim());

                    Lesson lesson = period.Lessons.SingleOrDefault(x => x.Name == name);
                    if (lesson != null)
                    {
                        lesson.Code = code;
                        lesson.Credit = credit;
                        lesson.AverageEffect = ganoEffect;
                        lesson.SuccessFactor = factor;
                        lesson.Class = cls;
                    }
                }
                if (i == col.Count - 1)
                    periods.Add(period);
            }
            return periods.OrderBy(x => x.YearCode).ThenBy(x => x.Code).ToList();
        }
        private static int GetYearCode(string year)
        {
            year = year.Split('-')[0];
            return (1976 - Convert.ToInt32(year));
        }
        private static int GetCode(string text)
        {
            text = text.ToUpper();
            switch (text)
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
