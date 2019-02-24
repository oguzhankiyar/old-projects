using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using Biletall.Helper.Parsings;

namespace Biletall.Helper.Requests
{
    public static class JourneyRequests
    {
        public static BaseRequest<List<Journey>> JourneysRequest;
        public static BaseRequest<PriceDetail> PriceDetailsRequest;

        static JourneyRequests()
        {
            JourneysRequest = BaseRequest<List<Journey>>.GetInstance();
            PriceDetailsRequest = BaseRequest<PriceDetail>.GetInstance();
        }

        public static void GetJourneys(TicketSearch search)
        {
            string xml;
            if (search.Type == TicketType.BusJourney)
                xml = JourneyParsings.GetJourneys(search);
            else
                xml = JourneyParsings.GetFlights(search);

            JourneysRequest.OnPopulated = (xmlResult) =>
            {
                if (search.Type == TicketType.BusJourney)
                    JourneysRequest.Response = JourneyParsings.ParseJourneys(xmlResult);
                else
                    JourneysRequest.Response = JourneyParsings.ParseFlights(xmlResult);
            };
            Global.OnRequestCalled("JourneyRequests.GetJourneys", new object[] { search });
            if (search.Type == TicketType.BusJourney)
                JourneysRequest.GetResult(xml, ApiAction.SearchBusTicket);
            else
                JourneysRequest.GetResult(xml, ApiAction.SearchAirplaneTicket);
        }

        public static void GetPriceDetails(Ticket ticket)
        {
            string xml = JourneyParsings.GetPriceDetails(ticket);
            PriceDetailsRequest.OnPopulated = (xmlResult) =>
            {
                PriceDetailsRequest.Response = JourneyParsings.ParsePriceDetails(xmlResult);
            };
            Global.OnRequestCalled("JourneyRequests.GetPriceDetails", new object[] { ticket });
            PriceDetailsRequest.GetResult(xml, ApiAction.GetAirplaneDetails);
        }

        public static void FillPassengerPrices(Ticket ticket, PriceDetail priceDetail)
        {
            Global.OnRequestCalled("JourneyRequests.FillPassengerPrices", new object[] { ticket, priceDetail });
            if (ticket == null || priceDetail == null)
                return;

            ticket.Is3DBuyingActivated = priceDetail.IsActivated3D;
            ticket.Is3DBuyingRequired = priceDetail.IsRequired3D;
            ticket.IsBirthdayRequired = priceDetail.IsRequiredBirthday;
            ticket.IsPassportRequired = priceDetail.IsRequiredPassport;

            foreach (var passenger in ticket.Passengers.ToList())
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

            Global.OnRequestCompleted();
        }
    }
}
