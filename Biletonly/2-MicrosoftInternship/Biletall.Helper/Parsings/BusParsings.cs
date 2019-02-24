using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Parsings
{
    internal class BusParsings
    {
        public static string GetBus(Segment segment)
        {
            var sb = new StringBuilder();
            sb.Append("<Otobus>");
            sb.Append("<FirmaNo>" + segment.Factory.Id + "</FirmaNo>");
            sb.Append("<KalkisAdi>" + segment.From.Name + "</KalkisAdi>");
            sb.Append("<VarisAdi>" + segment.To.Name + "</VarisAdi>");
            sb.Append("<Tarih>" + segment.DepartureDate.ToString("yyyy-MM-dd") + "</Tarih>");
            sb.Append("<Saat>" + segment.Hour + "</Saat>");
            sb.Append("<HatNo>" + segment.LineNumber + "</HatNo>");
            sb.Append("<IslemTipi>0</IslemTipi>");
            sb.Append("<SeferTakipNo>" + segment.Id + "</SeferTakipNo>");
            sb.Append("</Otobus>");
            return sb.ToString();
        }

        public static BaseResponse<Bus> ParseBus(string xml)
        {
            var response = new BaseResponse<Bus>();

            var doc = XDocument.Parse(xml);
            var root = doc.Element(XName.Get("Otobus"));
            if (root == null)
            {
                response.Status = ResponseStatus.Undefined;
                return response;
            }
            var bus = new Bus();
            var journey = root.Element(XName.Get("Sefer"));

            bus.DriverName = journey.Element(XName.Get("OtobusKaptanAdi")).Value.ToString();
            bus.Is3DBuyingActivated = journey.Element(XName.Get("Odeme3DSecureAktifMi")).Value.ToString() == "1";
            bus.Is3DSecureRequired = journey.Element(XName.Get("Odeme3DSecureZorunluMu")).Value.ToString() == "1";
            bus.Seats = ParseSeats(root.Elements().Where(x => x.Name.LocalName == "Koltuk")).Result;

            response.Result = bus;
            return response;
        }

        private static BaseResponse<List<Seat>> ParseSeats(IEnumerable<XElement> elements)
        {
            var response = new BaseResponse<List<Seat>>();

            var seats = new List<Seat>();
            foreach (var item in elements.ToList())
            {
                var seat = new Seat();
                seat.Name = item.Element(XName.Get("KoltukStr")).Value.ToString();
                seat.Number = Convert.ToInt32(item.Element(XName.Get("KoltukNo")).Value.ToString());
                seat.State = Functions.GetSeatState(item.Element(XName.Get("Durum")).Value.ToString());
                seat.NextState = Functions.GetSeatState(item.Element(XName.Get("DurumYan")).Value.ToString());
                seat.Price = new Price() { TotalPrice = Convert.ToDouble(item.Element(XName.Get("KoltukFiyatiInternet")).Value.ToString()) };
                seats.Add(seat);
            }

            response.Result = seats;
            return response;
        }

        public static BaseResponse<List<BusProperty>> ParseProperties(string value)
        {
            var response = new BaseResponse<List<BusProperty>>();

            var properties = new List<BusProperty>();
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].ToString() == "1")
                    properties.Add(Global.BusProperties[i]);
            }

            response.Result = properties;
            return response;
        }

        public static BaseResponse<List<BusProperty>> ParseAllProperties(IEnumerable<XElement> elements)
        {
            var response = new BaseResponse<List<BusProperty>>();

            var properties = new List<BusProperty>();
            foreach (var item in elements.ToList())
            {
                var property = new BusProperty();
                property.Index = Convert.ToInt32(item.Element(XName.Get("O_Tip_Ozellik")).Value.ToString());
                property.Name = item.Element(XName.Get("O_Tip_Ozellik_Aciklama")).Value.ToString();
                property.Description = item.Element(XName.Get("O_Tip_Ozellik_Detay")).Value.ToString();
                property.Icon = item.Element(XName.Get("O_Tip_Ozellik_Icon")).Value.ToString();
                properties.Add(property);
            }
            properties = properties.Where(x => !x.Name.Contains("Tanımsız")).ToList();

            response.Result = properties;
            return response;
        }

        public static string GetSeatStates(Ticket ticket)
        {
            var segment = ticket.Journeys[0].Segments[0];
            var sb = new StringBuilder();
            sb.Append("<WebAcente>");
            sb.Append("<WebAcenteIslemTip>2</WebAcenteIslemTip>");
            sb.Append("<FirmaNo>" + segment.Factory.Id + "</FirmaNo>");
            sb.Append("<KalkisAdi>" + segment.From.Name + "</KalkisAdi>");
            sb.Append("<VarisAdi>" + segment.To.Name + "</VarisAdi>");
            sb.Append("<Tarih>" + segment.DepartureDate.ToString("dd-MM-yyyy") + "</Tarih>");
            sb.Append("<Saat>" + segment.Hour + "</Saat>");
            sb.Append("<HatNo>" + segment.LineNumber + "</HatNo>");
            int i = 1;
            foreach (var passenger in ticket.Passengers.ToList())
            {
                sb.Append("<KoltukNo" + i + ">" + passenger.Seat.Number + "</KoltukNo" + i + ">");
                i++;
            }
            sb.Append("</WebAcente>");

            return sb.ToString();
        }

        public static BaseResponse<List<Seat>> ParseSeatStates(string xml)
        {
            var response = new BaseResponse<List<Seat>>();
            var seats = new List<Seat>();
            var root = XElement.Parse(xml);
            var seatNodes = root.Elements().Where(x => x.Name.LocalName == "Koltuk").ToList();
            foreach (var seatNode in seatNodes)
            {
                var seat = new Seat();
                seat.Number = Convert.ToInt32(seatNode.Element(XName.Get("KoltukNo")).Value.ToString());
                seat.State = Functions.GetSeatState(seatNode.Element(XName.Get("Durum")).Value.ToString());
                seat.NextState = Functions.GetSeatState(seatNode.Element(XName.Get("DurumYan")).Value.ToString());
                seats.Add(seat);
            }

            response.Result = seats;
            return response;
        }
    }
}
