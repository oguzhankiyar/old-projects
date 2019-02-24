using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biletall.Helper.Entities;
using Biletall.Helper.Parsings;

namespace Biletall.Helper.Requests
{
    public static class ServiceRequests
    {
        public static BaseRequest<List<ServiceStop>> FromServicesRequest;
        public static BaseRequest<List<ServiceStop>> ToServicesRequest;

        static ServiceRequests()
        {
            FromServicesRequest = BaseRequest<List<ServiceStop>>.GetInstance();
            ToServicesRequest = BaseRequest<List<ServiceStop>>.GetInstance();
        }

        public static void GetFromServices(Segment segment)
        {
            string xml = ServiceParsings.GetFromServices(segment);
            FromServicesRequest.OnPopulated = (xmlResult) =>
            {
                FromServicesRequest.Response = ServiceParsings.ParseServices(xmlResult);
            };
            Global.OnRequestCalled("ServiceRequests.GetFromServices", new object[] { segment });
            FromServicesRequest.GetResult(xml, ApiAction.GetServiceStops);
        }

        public static void GetToServices(Segment segment)
        {
            string xml = ServiceParsings.GetToServices(segment);
            ToServicesRequest.OnPopulated = (xmlResult) =>
            {
                ToServicesRequest.Response = ServiceParsings.ParseServices(xmlResult);
            };
            Global.OnRequestCalled("ServiceRequests.GetToServices", new object[] { segment });
            ToServicesRequest.GetResult(xml, ApiAction.GetServiceStops);
        }
    }
}
