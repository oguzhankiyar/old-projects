using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace OK.WebTools.Controllers
{
    public class Result
    {
        public string Url { get; set; }
        [AllowHtml]
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class XmlTree
    {
        public string Name { get; set; }
        public List<XmlTree> Items { get; set; }
        public string Value { get; set; }
    }
    public class FormatterController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Result obj)
        {
            if (!string.IsNullOrEmpty(obj.Url))
                obj.Text = getSource(obj.Url);
            else
                obj.Text = obj.Text.Trim();
            try
            {
                if (obj.Text[0] == '<')
                    obj.Value = ParseXml(obj.Text);
                else if (obj.Text[0] == '{')
                    obj.Value = ParseJObject(obj.Text);
                else
                    obj.Value = ParseJArray(obj.Text);
            }
            catch (Exception)
            {
                obj.Value = "Invalid format";
            }
            
            return View(obj);
        }
        
        private string ParseXml(string xml)
        {
            var result = new XmlTree();
            var root = XElement.Parse(xml);
            result.Name = root.Name.LocalName;
            result.Items = getItems(root);
            result.Value = root.Value;

            string res = "";
            if (!result.Items.Any())
                res = "<div class=\"value-row\"><div class=\"tag\"><p>" + result.Name + "</p></div><div class=\"value\"><p>" + getValue(result.Value) + "</p></div></div>";
            else
                res = "<div class=\"tree-row\"><span class=\"opened\"></span>&#60;" + result.Name + "&#62;<div class=\"inner\">" + getHtml(result.Items, 0) + "</div>&#60;/" + result.Name + "&#62;</div>";

            return res;
        }

        private string ParseJObject(string json)
        {
            string res = "";
            var root = JObject.Parse(json);
            var children = root.Children().ToList();
            foreach (JProperty child in children)
            {
                if (child.Value.ToString()[0] == '{')
                    res += "<div class=\"tree-row\"><span class=\"opened\"></span>" + child.Name + ":<div class=\"inner\">" + ParseJObject(child.Value.ToString()) + "</div></div>";
                else if (child.Value.ToString()[0] == '[')
                    res += "<div class=\"tree-row\"><span class=\"opened\"></span>" + child.Name + ":<div class=\"inner\">" + ParseJArray(child.Value.ToString()) + "</div></div>";
                else
                    res += "<div class=\"value-row\"><div class=\"tag\"><p>" + child.Name + "</p></div><div class=\"value\"><p>" + getValue(child.Value.ToString()) + "</p></div></div>";
            }
            return res;
        }

        private string ParseJArray(string json)
        {
            string res = "";
            var root = JArray.Parse(json);
            int i = 0;
            foreach (var item in root)
            {
                if (item.ToString()[0] == '{')
                    res += "<div class=\"tree-row\"><span class=\"opened\"></span>[" + i++ + "]<div class=\"inner\">" + ParseJObject(item.ToString()) + "</div></div>";
                else
                    res += "<div class=\"tree-row\"><span class=\"opened\"></span>[" + i++ + "]<div class=\"inner\"><div class=\"value-row\"><div class=\"value\"><p>" + item.ToString() + "</p></div></div></div></div>";
            }
            return res;
        }

        private string getSource(string url)
        {
            url = "http://" + url.Replace("http://", "");
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            var response = request.GetResponse();
            var sr = new StreamReader(response.GetResponseStream());
            return sr.ReadToEnd();
        }

        private string getHtml(List<XmlTree> result, int tab)
        {
            string res = "";
            foreach (var item in result)
            {
                if (!item.Items.Any())
                    res += "<div class=\"value-row\"><div class=\"tag\"><p>" + item.Name + "</p></div><div class=\"value\"><p>" + getValue(item.Value) + "</p></div></div>";
                else
                    res += "<div class=\"tree-row\"><span class=\"opened\"></span>&#60;" + item.Name + "&#62;<div class=\"inner\">" + getHtml(item.Items, 1) + "</div>&#60;/" + item.Name + "&#62;</div>";
            }
            return res;
        }

        private string getValue(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "<i class=\"empty\">(empty)</i>";
            return text;
        }
        private List<XmlTree> getItems(XElement root)
        {
            var items = new List<XmlTree>();
            foreach (var item in root.Elements())
            {
                var tree = new XmlTree();
                tree.Name = item.Name.LocalName;
                tree.Items = getItems(item);
                tree.Value = item.Value;
                items.Add(tree);
            }
            return items;
        }
    }
}