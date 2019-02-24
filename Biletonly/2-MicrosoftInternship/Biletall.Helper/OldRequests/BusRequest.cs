using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.BiletallService;
using Biletall.Helper.Entities;
using Biletall.Helper.Parsings;

namespace Biletall.Helper.Requests
{
    public class BusRequest
    {
        public static bool IsBusCompleted { get; private set; }
        public static Action<Bus> OnBusCompleted = null;

        public static bool IsSeatStatesCompleted { get; private set; }
        public static Action<List<Seat>> OnSeatStatesCompleted = null;

        public static void GetBus(Segment segment)
        {
            IsBusCompleted = false;
            string xml = BusParsing.GetBus(segment);
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (s, e) =>
            {
                string xmlResult = e.Result;
                PopulateBus(xmlResult);
            };
        }

        private static void PopulateBus(string xmlResult)
        {
            var bus = BusParsing.ParseBus(xmlResult);
            IsBusCompleted = true;
            if (OnBusCompleted != null)
                OnBusCompleted(bus);
        }

        public static void GetSeatStates(Journey journey)
        {
            IsSeatStatesCompleted = false;
            string xml = BusParsing.GetSeatStates(journey);
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (s, e) =>
            {
                string xmlResult = e.Result;
                PopulateSeatStates(xmlResult);
            };
        }

        private static void PopulateSeatStates(string xmlResult)
        {
            var seatStates = BusParsing.ParseSeatStates(xmlResult);
            IsSeatStatesCompleted = true;
            if (OnSeatStatesCompleted != null)
                OnSeatStatesCompleted(seatStates);
        }
    }
}
