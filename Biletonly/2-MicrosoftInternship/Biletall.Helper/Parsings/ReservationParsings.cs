using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Parsings
{
    internal class ReservationParsings
    {
        public static string GetBusReservation(Ticket ticket)
        {
            var segment = ticket.Journeys[0].Segments[0];
            var sb = new StringBuilder();
            sb.Append("<IslemRezervasyon>");
            sb.Append("<FirmaNo>" + segment.Factory.Id + "</FirmaNo>");
            sb.Append("<KalkisAdi>" + segment.From.Name + "</KalkisAdi>");
            sb.Append("<VarisAdi>" + segment.To.Name + "</VarisAdi>");
            sb.Append("<Tarih>" + segment.DepartureDate.ToString("yyyy-MM-dd") + "</Tarih>");
            sb.Append("<Saat>" + segment.Hour + "</Saat>");
            sb.Append("<HatNo>" + segment.LineNumber + "</HatNo>");
            sb.Append("<SeferNo>" + segment.Id + "</SeferNo>");
            for (int i = 1; i <= ticket.Passengers.Count(); i++)
            {
                var p = ticket.Passengers[i - 1] as Passenger;
                sb.Append("<KoltukNo" + i + ">" + p.Seat.Number + "</KoltukNo" + i + ">");
                sb.Append("<Adi" + i + ">" + Functions.TidyName(p.FirstName) + "</Adi" + i + ">");
                sb.Append("<Soyadi" + i + ">" + Functions.TidyName(p.LastName) + "</Soyadi" + i + ">");
                if (p.TCKN != null)
                    sb.Append("<TcKimlikNo" + i + ">" + p.TCKN.ToString() + "</TcKimlikNo" + i + ">");
                /*if (p.WhereCatch != null)
                    sb.Append("<BinecegiYer" + i + ">" + p.WhereCatch + "</BinecegiYer" + i + ">");*/
                if (p.FromServiceStop != null && p.FromServiceStop.Location != "Servis istemiyorum")
                    sb.Append("<ServisYeriKalkis" + i + ">" + p.FromServiceStop.Location.ToString() + "</ServisYeriKalkis" + i + ">");
                if (p.ToServiceStop != null && p.ToServiceStop.Location != "Servis istemiyorum")
                    sb.Append("<ServisYeriVaris" + i + ">" + p.ToServiceStop.Location.ToString() + "</ServisYeriVaris" + i + ">");
            }
            sb.Append("<TelefonNo>" + ticket.PNR.PhoneNumber.ToString() + "</TelefonNo> ");
            sb.Append("<Cinsiyet>" + (ticket.Passengers[0].Gender == Gender.Male ? 2 : 1) + "</Cinsiyet> ");
            sb.Append("<YolcuSayisi>" + ticket.Passengers.Count() + "</YolcuSayisi>");
            sb.Append("<WebYolcu>");
            sb.Append("<WebUyeNo>0</WebUyeNo>");
            sb.Append("<Ip>" + Functions.GetIPAddress().ToString() + "</Ip>");
            sb.Append("<Email>" + ticket.PNR.Email.ToString() + "</Email>");
            sb.Append("</WebYolcu>");
            sb.Append("</IslemRezervasyon>");
            return sb.ToString();
        }

        public static string GetAirplaneReservation(Ticket ticket)
        {
            var sb = new StringBuilder();
            sb.Append("<IslemUcak_2>");
            sb.Append("<IslemTip>1</IslemTip>");
            sb.Append("<FirmaNo>" + (ticket.Type == TicketType.InternationalJourney ? 1100 : 1000) + "</FirmaNo>");
            sb.Append("<TelefonNo>" + ticket.PNR.TelephoneNumber.ToString() + "</TelefonNo>");
            sb.Append("<CepTelefonNo>" + ticket.PNR.PhoneNumber.ToString() + "</CepTelefonNo>");
            sb.Append("<Email>" + ticket.PNR.Email.ToString() + "</Email>");
            sb.Append("<HatirlaticiNot />");

            int s = 1;
            foreach (var journey in ticket.Journeys.ToList())
            {
                foreach (var segment in journey.Segments.ToList())
                {
                    sb.Append("<Segment" + s + ">");
                    sb.Append("<Kalkis>" + segment.From.Code + "</Kalkis>");
                    sb.Append("<Varis>" + segment.To.Code + "</Varis>");
                    sb.Append("<KalkisTarih>" + segment.DepartureDate.ToString("s") + "</KalkisTarih>");
                    sb.Append("<VarisTarih>" + segment.ArrivalDate.ToString("s") + "</VarisTarih>");
                    sb.Append("<UcusNo>" + segment.Id + "</UcusNo>");
                    sb.Append("<FirmaKod>" + segment.Factory.Code + "</FirmaKod>");
                    sb.Append("<Sinif>" + segment.SelectedClass.ShortName.ToString() + "</Sinif>");
                    sb.Append("<DonusMu>" + (journey.IsReturn ? 1 : 0) + "</DonusMu>");
                    sb.Append("<SeferKod></SeferKod>");
                    sb.Append("</Segment" + s + ">");
                    s++;
                }
            }

            int p = 1;
            foreach (var passenger in ticket.Passengers.ToList())
            {
                sb.Append("<Yolcu" + p + ">");
                sb.Append("<Ad>" + Functions.TidyName(passenger.FirstName) + "</Ad>");
                sb.Append("<Soyad>" + Functions.TidyName(passenger.LastName) + "</Soyad>");
                sb.Append("<Cinsiyet>" + (passenger.Gender == Gender.Male ? 2 : 1) + "</Cinsiyet>");
                sb.Append("<YolcuTip>" + Functions.GetPassengerType(passenger.Type) + "</YolcuTip>");
                sb.Append("<TCKimlikNo>" + passenger.TCKN + "</TCKimlikNo>");
                if (ticket.IsBirthdayRequired)
                    sb.Append("<DogumTarih>" + ((DateTime)passenger.Birthday).ToString("yyyy-MM-dd") + "</DogumTarih>");
                if (ticket.IsPassportRequired)
                    sb.Append("<PasaportNo>" + passenger.PassaportId.ToString() + "</PasaportNo>");
                if (!string.IsNullOrEmpty(passenger.FactoryCardId))
                    sb.Append("<MilNo>" + passenger.FactoryCardId + "</MilNo>");
                else
                    sb.Append("<MilNo />");
                sb.Append("<NetFiyat>" + passenger.Price.NetPrice + "</NetFiyat>");
                sb.Append("<Vergi>" + passenger.Price.Tax + "</Vergi>");
                sb.Append("<ServisUcret>" + passenger.Price.ServicePrice + "</ServisUcret>");
                sb.Append("</Yolcu" + p + ">");
                p++;
            }
            sb.Append("</IslemUcak_2>");
            return sb.ToString();
        }

        public static BaseResponse<ActionResult> ParseReservation(string xml)
        {
            var response = new BaseResponse<ActionResult>();

            var root = XElement.Parse(xml);
            var actionResult = new ActionResult();
            if (root.Element(XName.Get("Sonuc")).Value == "true")
            {
                actionResult.PNR = root.Element(XName.Get("PNR")).Value;
                actionResult.ExpirationDate = DateTime.ParseExact(root.Element(XName.Get("RezervasyonOpsiyon")).Value.Replace(":", ".").ToString(), "dd.MM.yyyy HH.mm", null);
                if (root.Element(XName.Get("Mesaj")) != null)
                    response.Message = root.Element(XName.Get("Mesaj")).Value;
            }
            else
            {
                response.Status = ResponseStatus.Undefined;
                response.Message = root.Element(XName.Get("Hata")).Value;
            }

            response.Result = actionResult;
            return response;
        }

        public static string GetCancelReservation(Ticket ticket, Passenger passenger)
        {
            var sb = new StringBuilder();
            sb.Append("<PnrIslem>");
            sb.Append("<PnrNo>" + ticket.PNR.Code + "</PnrNo>");
            if (ticket.Type == TicketType.BusJourney)
            {
                if (passenger == null)
                    sb.Append("<PnrKoltukNo>0</PnrKoltukNo>");
                else
                    sb.Append("<PnrKoltukNo>" + passenger.Seat.Number + "</PnrKoltukNo>");
            }
            sb.Append("<WebUyeNo>0</WebUyeNo>");
            sb.Append("<PnrIslemTip>1</PnrIslemTip>");
            sb.Append("</PnrIslem>");
            return sb.ToString();
        }

        public static BaseResponse<ActionResult> ParseCancelReservation(string xmlResult)
        {
            var response = new BaseResponse<ActionResult>();

            var root = XElement.Parse(xmlResult);
            string result = root.Element(XName.Get("Sonuc")).Value.ToString();
            var actionResult = new ActionResult();
            if (root.Element(XName.Get("Sonuc")).Value != "true")
                response.Status = ResponseStatus.Undefined;

            response.Result = actionResult;
            return response;
        }

    }
}
