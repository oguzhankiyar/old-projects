using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using HtmlAgilityPack;

namespace ObisisService.Models
{
    [DataContract]
    public class Document : ObjectModel
    {
        [DataMember]
        public ProgramType Program { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public DateTime RequestDate { get; set; }
        [DataMember]
        public int RequestCount { get; set; }
        [DataMember]
        public DateTime PrintDate { get; set; }
        [DataMember]
        public int PrintCount { get; set; }
        [DataMember]
        public bool IsPrinted { get; set; }

        public static List<Document> GetDocuments(string html)
        {
            List<Document> documents = new List<Document>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            HtmlNodeCollection col = node.SelectNodes("//table[@id='ctl00_dgBelge']//tr");
            for (int i = 1; i < col.Count; i++)
            {
                HtmlNode item = col[i];
                Document document = new Document();
                document.Program = item.SelectSingleNode("./td[1]").InnerText.Contains("ANADAL") ? ProgramType.Major : ProgramType.Minor;
                document.Type = item.SelectSingleNode("./td[2]").InnerText;
                document.RequestDate = Convert.ToDateTime(item.SelectSingleNode("./td[3]").InnerText);
                document.RequestCount = Convert.ToInt32(item.SelectSingleNode("./td[4]").InnerText);
                document.IsPrinted = false;
                if (!string.IsNullOrEmpty(Function.TidyText(item.SelectSingleNode("./td[5]").InnerText)))
                {
                    document.IsPrinted = true;
                    document.PrintDate = Convert.ToDateTime(item.SelectSingleNode("./td[5]").InnerText);
                    document.PrintCount = Convert.ToInt32(item.SelectSingleNode("./td[6]").InnerText);
                }
                documents.Add(document);
            }
            return documents;
        }
    }
}
