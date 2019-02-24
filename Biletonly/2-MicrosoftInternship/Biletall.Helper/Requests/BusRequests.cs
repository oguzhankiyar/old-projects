using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biletall.Helper.Entities;
using Biletall.Helper.Parsings;

namespace Biletall.Helper.Requests
{
    public static class BusRequests
    {
        public static BaseRequest<Bus> BusRequest;
        public static BaseRequest<List<Seat>> SeatStatesRequest;

        static BusRequests()
        {
            BusRequest = BaseRequest<Bus>.GetInstance();
            SeatStatesRequest = BaseRequest<List<Seat>>.GetInstance();
        }

        public static void GetBus(Segment segment)
        {
            string xml = BusParsings.GetBus(segment);
            BusRequest.OnPopulated = (xmlResult) =>
            {
                BusRequest.Response = BusParsings.ParseBus(xmlResult);
            };
            Global.OnRequestCalled("BusRequests.GetBus", new object[] { segment });
            BusRequest.GetResult(xml, ApiAction.GetBusDetails);
        }

        public static void GetSeatStates(Ticket ticket)
        {
            string xml = BusParsings.GetSeatStates(ticket);
            SeatStatesRequest.OnPopulated = (xmlResult) =>
            {
                SeatStatesRequest.Response = BusParsings.ParseSeatStates(xmlResult);
            };
            Global.OnRequestCalled("BusRequests.GetSeatStates", new object[] { ticket });
            SeatStatesRequest.GetResult(xml, ApiAction.GetBusSeatStates);
        }
    }
}
