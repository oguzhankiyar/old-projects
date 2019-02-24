using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.BiletallService;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using Biletall.Helper.Parsings;

namespace Biletall.Helper.Requests
{
    public class JourneyRequest
    {
        #region BusJourney
        public static bool IsJourneysCompleted { get; private set; }
        public static Action<List<Journey>> OnJourneysCompleted = null;

        public static void GetJourneys(Journey journey)
        {
            IsJourneysCompleted = false;
            string xml;
            if (journey.From.Type == StationType.Bus)
                xml = JourneyParsing.GetJourneys(journey);
            else
                xml = JourneyParsing.GetFlights(journey);
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (s, e) =>
            {
                string xmlResult = e.Result;
                if (journey.From.Type == StationType.Bus)
                    PopulateJourneys(xmlResult);
                else
                    PopulateFlights(xmlResult);
            };
        }

        private static void PopulateJourneys(string xmlResult)
        {
            var journeys = JourneyParsing.ParseJourneys(xmlResult);
            IsJourneysCompleted = true;
            if (OnJourneysCompleted != null)
                OnJourneysCompleted(journeys);
        }
        #endregion

        #region AirplaneJourney
        private static void PopulateFlights(string xmlResult)
        {
            var flights = JourneyParsing.ParseFlights(xmlResult);
            IsJourneysCompleted = true;
            if (OnJourneysCompleted != null)
                OnJourneysCompleted(flights);
        }

        public static bool IsPriceDetailsCompleted = false;
        public static Action<PriceDetail> OnPriceDetailsCompleted = null;

        public static void GetPriceDetails(Journey journey)
        {
            IsPriceDetailsCompleted = false;
            string xml = JourneyParsing.GetPriceDetails(journey);
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (c, e) =>
            {
                string xmlResult = e.Result;
                PopulatePriceDetails(xmlResult);
            };
        }

        private static void PopulatePriceDetails(string xmlResult)
        {
            var details = JourneyParsing.ParsePriceDetails(xmlResult);
            IsPriceDetailsCompleted = true;
            if (OnPriceDetailsCompleted != null)
                OnPriceDetailsCompleted(details);
        }

        public static void FillPassengerPrices(Ticket ticket, PriceDetail priceDetail)
        {
            ticket.IsActivated3D = priceDetail.IsActivated3D;
            ticket.IsRequired3D = priceDetail.IsRequired3D;
            ticket.IsRequiredBirthday = priceDetail.IsRequiredBirthday;
            ticket.IsRequiredPassport = priceDetail.IsRequiredPassport;
            if (ticket.Journey == null)
                return;

            foreach (var passenger in ticket.Journey.Passengers.ToList())
            {
                switch (passenger.Type)
                {
                    case PassengerType.Adult:
                        int adultCount = priceDetail.AdultPrice.PassengerCount;
                        passenger.Price = new Price();
                        passenger.Price.NetPrice = priceDetail.AdultPrice.NetPrice / adultCount;
                        passenger.Price.Tax = priceDetail.AdultPrice.Tax / adultCount;
                        passenger.Price.ServicePrice = priceDetail.AdultPrice.ServicePrice / adultCount;
                        break;
                    case PassengerType.Child:
                        int childCount = priceDetail.ChildPrice.PassengerCount;
                        passenger.Price = new Price();
                        passenger.Price.NetPrice = priceDetail.ChildPrice.NetPrice / childCount;
                        passenger.Price.Tax = priceDetail.ChildPrice.Tax / childCount;
                        passenger.Price.ServicePrice = priceDetail.ChildPrice.ServicePrice / childCount;
                        break;
                    case PassengerType.Baby:
                        int babyCount = priceDetail.BabyPrice.PassengerCount;
                        passenger.Price = new Price();
                        passenger.Price.NetPrice = priceDetail.BabyPrice.NetPrice / babyCount;
                        passenger.Price.Tax = priceDetail.BabyPrice.Tax / babyCount;
                        passenger.Price.ServicePrice = priceDetail.BabyPrice.ServicePrice / babyCount;
                        break;
                    case PassengerType.Infant:
                        int infantCount = priceDetail.InfantPrice.PassengerCount;
                        passenger.Price = new Price();
                        passenger.Price.NetPrice = priceDetail.InfantPrice.NetPrice / infantCount;
                        passenger.Price.Tax = priceDetail.InfantPrice.Tax / infantCount;
                        passenger.Price.ServicePrice = priceDetail.InfantPrice.ServicePrice / infantCount;
                        break;
                    case PassengerType.Student:
                        int studentCount = priceDetail.StudentPrice.PassengerCount;
                        passenger.Price = new Price();
                        passenger.Price.NetPrice = priceDetail.StudentPrice.NetPrice / studentCount;
                        passenger.Price.Tax = priceDetail.StudentPrice.Tax / studentCount;
                        passenger.Price.ServicePrice = priceDetail.StudentPrice.ServicePrice / studentCount;
                        break;
                    case PassengerType.Military:
                        int militaryCount = priceDetail.MilitaryPrice.PassengerCount;
                        passenger.Price = new Price();
                        passenger.Price.NetPrice = priceDetail.MilitaryPrice.NetPrice / militaryCount;
                        passenger.Price.Tax = priceDetail.MilitaryPrice.Tax / militaryCount;
                        passenger.Price.ServicePrice = priceDetail.MilitaryPrice.ServicePrice / militaryCount;
                        break;
                    case PassengerType.Teenager:
                        int teenagerCount = priceDetail.TeenagerPrice.PassengerCount;
                        passenger.Price = new Price();
                        passenger.Price.NetPrice = priceDetail.TeenagerPrice.NetPrice / teenagerCount;
                        passenger.Price.Tax = priceDetail.TeenagerPrice.Tax / teenagerCount;
                        passenger.Price.ServicePrice = priceDetail.TeenagerPrice.ServicePrice / teenagerCount;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
    }
}
