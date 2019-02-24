using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.BiletallService;
using Biletall.Helper.Entities;
using Biletall.Helper.Parsings;

namespace Biletall.Helper.Requests
{
    public class StationRequest
    {
        #region FromStations
        public static bool IsFromStationsCompleted { get; private set; }
        public static Action<List<Station>> OnFromStationsCompleted = null;

        public static void GetFromStations()
        {
            IsFromStationsCompleted = false;
            string xml = StationParsing.GetFromStations();
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (c, e) =>
            {
                string xmlResult = e.Result;
                PopulateFromStations(xmlResult);
            };
        }

        private static void PopulateFromStations(string xmlResult)
        {
            var stations = StationParsing.ParseFromStations(xmlResult);
            IsFromStationsCompleted = true;
            if (OnFromStationsCompleted != null)
                OnFromStationsCompleted(stations);
        }
        #endregion

        #region ToStations
        public static bool IsToStationsCompleted { get; private set; }
        public static Action<List<Station>> OnToStationsCompleted = null;

        public static void GetToStations(Station fromStation)
        {
            IsToStationsCompleted = false;
            string xml = StationParsing.GetToStations(fromStation);
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (c, e) =>
            {
                string xmlResult = e.Result;
                PopulateToStations(xmlResult);
            };
        }

        private static void PopulateToStations(string xmlResult)
        {
            var stations = StationParsing.ParseToStations(xmlResult);
            IsToStationsCompleted = true;
            if (OnToStationsCompleted != null)
                OnToStationsCompleted(stations);
        }
        #endregion

        #region Airports
        public static bool IsAirportsCompleted { get; private set; }
        public static Action<List<Station>> OnAirportsCompleted = null;

        public static void GetAirports()
        {
            IsAirportsCompleted = false;
            string xml = StationParsing.GetAirports();
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (c, e) =>
            {
                string xmlResult = e.Result;
                PopulateAirports(xmlResult);
            };
        }

        private static void PopulateAirports(string xmlResult)
        {
            var stations = StationParsing.ParseAirports(xmlResult);
            IsAirportsCompleted = true;
            if (OnAirportsCompleted != null)
                OnAirportsCompleted(stations);
        }
        #endregion

        public static void SetCompleted()
        {
            IsFromStationsCompleted = IsAirportsCompleted = true;
        }
    }
}