using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Parsings
{
    internal class StationParsings
    {
        #region FromStations
        public static string GetFromStations()
        {
            var sb = new StringBuilder();
            sb.Append("<Kalkis>");
            sb.Append("<FirmaNo>" + 0 + "</FirmaNo>");
            sb.Append("<IlceleriGoster>" + 1 + "</IlceleriGoster>");
            sb.Append("</Kalkis>");
            return sb.ToString();
        }

        public static BaseResponse<List<Station>> ParseFromStations(string xml)
        {
            var response = new BaseResponse<List<Station>>();

            var doc = XDocument.Parse(xml);
            var root = doc.Element(XName.Get("NewDataSet"));
            var elements = root.Elements().Where(x => x.Name.LocalName == "Table");

            var stations = new List<Station>();
            foreach (var item in elements)
            {
                stations.Add(new Station() { Name = item.Element(XName.Get("Kalkis_Yeri")).Value.ToString(), Type = StationType.Bus });
            }

            response.Result = stations;
            return response;
        }
        #endregion

        #region ToStations
        public static string GetToStations(Station fromStation)
        {
            var sb = new StringBuilder();
            sb.Append("<Varis>");
            sb.Append("<FirmaNo>" + 0 + "</FirmaNo>");
            sb.Append("<IlceleriGoster>" + 1 + "</IlceleriGoster>");
            sb.Append("<KalkisAdi>" + fromStation.Name + "</KalkisAdi>");
            sb.Append("</Varis>");
            return sb.ToString();
        }

        public static BaseResponse<List<Station>> ParseToStations(string xml)
        {
            var response = new BaseResponse<List<Station>>();

            var doc = XDocument.Parse(xml);
            var root = doc.Element(XName.Get("NewDataSet"));
            var elements = root.Elements().Where(x => x.Name.LocalName == "Table");

            var stations = new List<Station>();
            foreach (var item in elements)
            {
                stations.Add(new Station() { Name = item.Element(XName.Get("Varis_Yeri")).Value.ToString(), Type = StationType.Bus });
            }

            response.Result = stations;
            return response;
        }
        #endregion

        #region Airports
        public static string GetAirports()
        {
            return "<Kalkis><HavaAlan>1</HavaAlan></Kalkis>";
        }

        public static BaseResponse<List<Station>> ParseAirports(string xml)
        {
            var response = new BaseResponse<List<Station>>();

            var root = XElement.Parse(xml);
            var elements = root.Elements().Where(x => x.Name.LocalName == "Table");

            var stations = new List<Station>();
            foreach (var item in elements.ToList())
            {
                var station = new Station();
                station.Code = item.Element(XName.Get("Kod")).Value.ToString();
                station.Name = item.Element(XName.Get("HavaAlan")).Value.ToString();
                station.City = item.Element(XName.Get("Sehir")).Value.ToString();
                station.Country = item.Element(XName.Get("Ulke")).Value.ToString();
                station.IsInternational = item.Element(XName.Get("ID")).Value.ToString() != "1";
                station.Type = StationType.Airplane;
                stations.Add(station);
            }

            response.Result = stations;
            return response;
        }
        #endregion
    }
}
