using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace ObisisService
{
    [DataContract]
    public class YemekList
    {
        [DataMember]
        public string Tarih { get; set; }

        [DataMember]
        public string Ogun { get; set; }

        [DataMember]
        public List<Yemek> Yemekler { get; set; }

        public static List<YemekList> GetList()
        {
            List<YemekList> yemekList = new List<YemekList>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Function.GetHTML("http://www.erciyes.edu.tr/tr/yemek/oyemek.asp", null, null, "windows-1254"));

            string txt = doc.DocumentNode.SelectSingleNode("//tr[1]").InnerText;
            txt = txt.Replace("\r", null).Replace("\t", null).Replace("\n", "<%%>");
            var rx = new Regex("((?<1>((?!<%).)+)|<%(?<2>((?!%>).)*)%>)*", RegexOptions.ExplicitCapture);
            var res2 = rx.Match(txt);

            string[] text = res2.Groups[1].Captures.Cast<Capture>().Select(p => p.Value).ToArray();

            int Count = 0;
            YemekList yemek1 = new YemekList();
            YemekList yemek2 = new YemekList();
            List<Yemek> yemekler1 = new List<Yemek>();
            List<Yemek> yemekler2 = new List<Yemek>();
            foreach (string item in text)
            {
                string myItem = item.Trim();
                if (myItem.Length > 10 && !myItem.Contains("Öğrenci Yemek Listesi") && !myItem.Contains("<!"))
                {
                    Count++;
                    myItem = Function.TidyText(myItem.Replace("ÖĞLE", "").Replace("AKŞAM", ""));
                    if (Count % 14 == 1)
                    {
                        yemek1 = new YemekList();
                        yemek1.Tarih = Convert.ToDateTime(myItem.Split(' ')[0]).ToShortDateString();
                    }
                    else if (Count % 14 == 2)
                    {
                        yemek2 = new YemekList();
                        yemek2.Tarih = Convert.ToDateTime(myItem.Split(' ')[0]).ToShortDateString();
                    }
                    else if (Count % 14 == 3)
                    {
                        yemekler1 = new List<Yemek>();
                        Yemek yemek = new Yemek();
                        yemek.Adi = myItem.Split('(')[0];
                        yemek.Kalori = myItem.Split('(')[1].ToLower().Replace("kcal)", ""); 
                        yemekler1.Add(yemek);
                    }
                    else if (Count % 14 == 4)
                    {
                        Yemek yemek = new Yemek();
                        yemek.Adi = myItem.Split('(')[0];
                        yemek.Kalori = myItem.Split('(')[1].ToLower().Replace("kcal)", "");
                        yemekler1.Add(yemek);
                    }
                    else if (Count % 14 == 5)
                    {
                        Yemek yemek = new Yemek();
                        yemek.Adi = myItem.Split('(')[0];
                        yemek.Kalori = myItem.Split('(')[1].ToLower().Replace("kcal)", "");
                        yemekler1.Add(yemek);

                        yemek1.Yemekler = yemekler1;
                        yemek1.Ogun = "Öğle - Akşam";
                        yemekList.Add(yemek1);
                    }
                    else if (Count % 14 == 9)
                    {
                        yemekler2 = new List<Yemek>();
                        Yemek yemek = new Yemek();
                        yemek.Adi = myItem.Split('(')[0];
                        yemek.Kalori = myItem.Split('(')[1].ToLower().Replace("kcal)", "");
                        yemekler2.Add(yemek);
                    }
                    else if (Count % 14 == 10)
                    {
                        Yemek yemek = new Yemek();
                        yemek.Adi = myItem.Split('(')[0];
                        yemek.Kalori = myItem.Split('(')[1].ToLower().Replace("kcal)", "");
                        yemekler2.Add(yemek);
                    }
                    else if (Count % 14 == 11)
                    {
                        Yemek yemek = new Yemek();
                        yemek.Adi = myItem.Split('(')[0];
                        yemek.Kalori = myItem.Split('(')[1].ToLower().Replace("kcal)", "");
                        yemekler2.Add(yemek);

                        yemek2.Yemekler = yemekler2;
                        yemek2.Ogun = "Öğle - Akşam";
                        yemekList.Add(yemek2);
                    }
                }
            }
            return yemekList.OrderBy(x => x.Tarih).ToList();
            /*
             
             List<YemekList> yemekList = new List<YemekList>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Function.GetHTML("http://www.erciyes.edu.tr/tr/yemek/oyemek.asp"));
            HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//body//table[1]//td");


            int trCount = 0;
            YemekList yemek1 = new YemekList();
            YemekList yemek2 = new YemekList();
            List<Yemek> yemekler1 = new List<Yemek>();
            List<Yemek> yemekler2 = new List<Yemek>();
            foreach (HtmlNode item in nodeCollection)
            {
                if (item.InnerHtml.Contains("<!") == false && item.InnerHtml.Contains("<td") == false && item.InnerText.Replace("\r", null).Replace("\n", null).Replace(" ", null).Replace("&nbsp;", null) != "")
                {
                    string text = item.InnerText.Replace("\r", null).Replace("\n", null);
                    ++trCount;
                    if (trCount % 4 == 1)
                    {
                        yemek1.Tarih = Convert.ToDateTime(text.Split(' ')[0]);
                    }
                    else if (trCount % 4 == 2)
                    {
                        yemek2.Tarih = Convert.ToDateTime(text.Split(' ')[0]);
                    }
                    else if (trCount % 4 == 3)
                    {
                        Yemek yemek = new Yemek() { Adi = text };
                        yemekler1.Add(yemek);
                        yemek1.Yemekler = yemekler1;
                        yemek1.Ogun = item.SelectSingleNode("./b[1]").InnerText;

                        yemek2.Yemekler = yemekler1;
                        yemek2.Ogun = item.SelectSingleNode("./b[1]").InnerText;
                        yemekList.Add(yemek1);
                        yemekList.Add(yemek2);
                    }
                    else
                    {
                        Yemek yemek = new Yemek() { Adi = text };
                        yemekler1.Add(yemek);
                        yemek1.Yemekler = yemekler1;
                        yemek1.Ogun = item.SelectSingleNode("./b[1]").InnerText;

                        yemek2.Yemekler = yemekler1;
                        yemek2.Ogun = item.SelectSingleNode("./b[1]").InnerText;
                        yemekList.Add(yemek1);
                        yemekList.Add(yemek2);
                    }
                }
            }
            return yemekList;
             */

        }
        [DataContract]
        public class Yemek
        {
            [DataMember]
            public string Adi { get; set; }

            [DataMember]
            public string Kalori { get; set; }
        }
    }
}