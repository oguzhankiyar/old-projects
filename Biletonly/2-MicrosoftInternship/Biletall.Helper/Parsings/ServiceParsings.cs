using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Biletall.Helper.Entities;

namespace Biletall.Helper.Parsings
{
    internal class ServiceParsings
    {
        public static string GetFromServices(Segment segment)
        {
            var sb = new StringBuilder();
            sb.Append("<Servis>");
            sb.Append("<FirmaNo>" + segment.Factory.Id + "</FirmaNo>");
            sb.Append("<KalkisAdi>" + segment.From.Name + "</KalkisAdi>");
            sb.Append("<YerelSaat>" + segment.Hour + "</YerelSaat>");
            sb.Append("<HatNo>" + segment.LineNumber + "</HatNo>");
            sb.Append("</Servis>");
            return sb.ToString();
        }

        public static string GetToServices(Segment segment)
        {
            var sb = new StringBuilder();
            sb.Append("<Servis>");
            sb.Append("<FirmaNo>" + segment.Factory.Id + "</FirmaNo>");
            sb.Append("<KalkisAdi>" + segment.To.Name + "</KalkisAdi>");
            sb.Append("<YerelSaat>" + segment.Hour + "</YerelSaat>");
            sb.Append("<HatNo>" + segment.LineNumber + "</HatNo>");
            sb.Append("</Servis>");
            return sb.ToString();
        }

        public static BaseResponse<List<ServiceStop>> ParseServices(string xml)
        {
            var response = new BaseResponse<List<ServiceStop>>();

            var services = new List<ServiceStop>();
            var doc = XDocument.Parse(xml);

            if (doc.Element(XName.Get("IslemSonuc")) == null)
            {
                var element = doc.Element(XName.Get("NewDataSet"));
                var elements = element.Elements().Where(x => x.Name.LocalName == "Table").ToList();

                foreach (var item in elements)
                {
                    var service = new ServiceStop();
                    service.Location = item.Element(XName.Get("Yer")).Value.ToString();
                    service.Hour = Convert.ToDateTime(item.Element(XName.Get("Saat")).Value.ToString());
                    if (!string.IsNullOrEmpty(service.Location))
                        services.Add(service);
                }
            }
            if (services.Count() > 0)
                services.Insert(0, new ServiceStop() { Location = "Servis istemiyorum", Hour = null });

            response.Result = services;
            return response;
        }
    }
}
