using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using HtmlAgilityPack;

namespace ObisisService.Models
{
    [DataContract]
    public class Graduation : ObjectModel
    {
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public bool Value { get; set; }
        [DataMember]
        public String State { get; set; }
        [DataMember]
        public String Condition { set; get; }

        public static List<Graduation> GetGraduationState(string html)
        {
            List<Graduation> list = new List<Graduation>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            HtmlNodeCollection col = node.SelectNodes("//table[@id='ctl00_dgMezuniyetDurum']//tr");
            for (int i = 1; i < col.Count; i++)
            {
                HtmlNode item = col[i];
                if (!string.IsNullOrEmpty(item.InnerText.Replace("&nbsp;", "").Trim()) && !item.InnerText.Contains("SONUÇ"))
                {
                    Graduation grade = new Graduation();
                    grade.Condition = Function.TidyText(item.SelectSingleNode("./td[1]").InnerText);
                    grade.State = Function.TidyText(item.SelectSingleNode("./td[2]").InnerText);
                    grade.Value = !item.SelectSingleNode("./td[3]").InnerText.Contains("InValid");
                    grade.Description = Function.TidyText(item.SelectSingleNode("./td[4]").InnerText);
                    list.Add(grade);
                }
            }
            return list;
        }
    }
}
