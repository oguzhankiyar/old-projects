using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biletall.Helper.Entities;
using Biletall.Helper.Parsings;

namespace Biletall.Helper.Requests
{
    public static class StationRequests
    {
        public static BaseRequest<List<Station>> FromStationsRequest;
        public static BaseRequest<List<Station>> ToStationsRequest;
        public static BaseRequest<List<Station>> AirportsRequest;
        
        static StationRequests()
        {
            FromStationsRequest = BaseRequest<List<Station>>.GetInstance();
            ToStationsRequest = BaseRequest<List<Station>>.GetInstance();
            AirportsRequest = BaseRequest<List<Station>>.GetInstance();
        }

        public static void GetFromStations()
        {
            string xml = StationParsings.GetFromStations();
            FromStationsRequest.OnPopulated = (xmlResult) =>
            {
                FromStationsRequest.Response = StationParsings.ParseFromStations(xmlResult);
            };
            Global.OnRequestCalled("StationRequests.GetFromStations", new object[] { });
            FromStationsRequest.GetResult(xml, ApiAction.GetBusFromStations);
        }

        public static void GetToStations(Station fromStation)
        {
            string xml = StationParsings.GetToStations(fromStation);
            ToStationsRequest.OnPopulated = (xmlResult) =>
            {
                ToStationsRequest.Response = StationParsings.ParseToStations(xmlResult);
            };
            Global.OnRequestCalled("StationRequests.GetToStations", new object[] { fromStation });
            ToStationsRequest.GetResult(xml, ApiAction.GetBusToStations);
        }

        public static void GetAirports()
        {
            string xml = StationParsings.GetAirports();
            AirportsRequest.OnPopulated = (xmlResult) =>
            {
                AirportsRequest.Response = StationParsings.ParseAirports(xmlResult);
            };
            Global.OnRequestCalled("StationRequests.GetAirports", new object[] { });
            AirportsRequest.GetResult(xml, ApiAction.GetAirplaneStations);
        }
    }
}