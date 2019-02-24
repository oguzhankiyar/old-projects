using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using HtmlAgilityPack;

namespace ObisisService.Models
{
    [DataContract]
    public class Data
    {
        [DataMember]
        public static int StudentID { get; set; }
        [DataMember]
        public static string Password { get; set; }
        [DataMember]
        public static string GraduationURL { get; set; }
        [DataMember]
        public static string LessonStateURL { get; set; }
        [DataMember]
        public static string DocumentsURL { get; set; }
        [DataMember]
        public static string LessonsURL { get; set; }

        public static void GetURLs(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            LessonsURL = HttpUtility.HtmlDecode("http://obisis.erciyes.edu.tr/" + node.SelectSingleNode("//a[contains(text(), 'Not Sorgu')]").Attributes["href"].Value);
            GraduationURL = HttpUtility.HtmlDecode("http://obisis.erciyes.edu.tr/" + node.SelectSingleNode("//a[contains(text(), 'Mezuniyet')]").Attributes["href"].Value);
            DocumentsURL = HttpUtility.HtmlDecode("http://obisis.erciyes.edu.tr/" + node.SelectSingleNode("//a[contains(text(), 'Belge İstek')]").Attributes["href"].Value);
            LessonStateURL = HttpUtility.HtmlDecode("http://obisis.erciyes.edu.tr/" + node.SelectSingleNode("//a[contains(text(), 'Ders Durum')]").Attributes["href"].Value);
        }

    }
}