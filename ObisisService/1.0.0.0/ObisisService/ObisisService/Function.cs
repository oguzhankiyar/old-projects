using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ObisisService
{
    public class Function
    {
        public static string GetHTML(string url, string ogrNo = null, string sifre = null, string encoding = null)
        {
            CookieContainer _CookieContainer_ = new CookieContainer();
            if (ogrNo != null || sifre != null)
            {
                string firstURL = "http://obisis.erciyes.edu.tr";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(firstURL);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader stream = new StreamReader(response.GetResponseStream());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(stream.ReadToEnd());
                HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//input");
                string postData = string.Format("__VIEWSTATE={0}&__EVENTVALIDATION={1}&ctl00$txtboxOgrenciNo={2}&ctl00$txtBoxSifre={3}&ctl00$btnLogin={4}", HttpUtility.UrlEncode(collection[1].Attributes["value"].Value), HttpUtility.UrlEncode(collection[2].Attributes["value"].Value), ogrNo, sifre, HttpUtility.UrlEncode("Giriş"));

                HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(firstURL);
                _request.CookieContainer = _CookieContainer_;
                _request.Method = "POST";
                _request.ContentType = "application/x-www-form-urlencoded";
                _request.KeepAlive = true;
                byte[] byteArray = Encoding.ASCII.GetBytes(postData);
                _request.ContentLength = byteArray.Length;
                Stream newStream = _request.GetRequestStream(); //open connection
                newStream.Write(byteArray, 0, byteArray.Length); // Send the data.
                newStream.Close();
                HttpWebResponse getResponse = (HttpWebResponse)_request.GetResponse();
            }
            HttpWebRequest _req = (HttpWebRequest)WebRequest.Create(url);
            _req.CookieContainer = _CookieContainer_;
            HttpWebResponse _res = (HttpWebResponse)_req.GetResponse();
            StreamReader _stream;
            if (encoding == null)
                _stream = new StreamReader(_res.GetResponseStream());
            else
                _stream = new StreamReader(_res.GetResponseStream(), Encoding.GetEncoding("windows-1254"));
            return (_stream.ReadToEnd());
        }
        public static string TidyText(string text)
        {
            text = text.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("-", " - ").Replace("/", " / ").Replace("  ", "");
                string[] split = text.Split(' ');
                text = "";
                for (int i = 0; i < split.Length; i++)
                {
                    char[] chars = split[i].ToCharArray();
                    split[i] = chars[0].ToString().ToUpper();
                    for (int j = 1; j < chars.Length; j++)
                    {
                        split[i] += chars[j].ToString().ToLower();
                    }
                    text += split[i] + " ";
                }
                text = text.Replace("VE", "ve");
                text = text.Replace("ıı", "II");
                text = text.Replace("Iı", "II");
            }
            return text.Trim();
        }
    }
}