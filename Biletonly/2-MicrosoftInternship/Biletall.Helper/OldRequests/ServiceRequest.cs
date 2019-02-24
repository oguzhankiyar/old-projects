using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.BiletallService;
using Biletall.Helper.Entities;
using Biletall.Helper.Parsings;

namespace Biletall.Helper.Requests
{
    public class ServiceRequest
    {
        #region FromServices
        public static bool IsFromServicesCompleted { get; private set; }
        public static Action<List<ServiceStop>> OnFromServicesCompleted = null;

        public static void GetFromServices(Segment segment)
        {
            IsFromServicesCompleted = false;
            string xml = ServiceParsing.GetFromServices(segment);
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (s, e) =>
            {
                string xmlResult = e.Result;
                PopulateFromServices(xmlResult);
            };
        }

        private static void PopulateFromServices(string xmlResult)
        {
            var services = ServiceParsing.ParseServices(xmlResult);
            IsFromServicesCompleted = true;
            if (OnFromServicesCompleted != null)
                OnFromServicesCompleted(services);
        }
        #endregion

        #region ToServices
        public static bool IsToServicesCompleted { get; private set; }
        public static Action<List<ServiceStop>> OnToServicesCompleted = null;

        public static void GetToServices(Segment segment)
        {
            IsToServicesCompleted = false;
            string xml = ServiceParsing.GetToServices(segment);
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (s, e) =>
            {
                string xmlResult = e.Result;
                PopulateToServices(xmlResult);
            };
        }

        private static void PopulateToServices(string xmlResult)
        {
            var services = ServiceParsing.ParseServices(xmlResult);
            IsToServicesCompleted = true;
            if (OnToServicesCompleted != null)
                OnToServicesCompleted(services);
        }
        #endregion
    }
}
