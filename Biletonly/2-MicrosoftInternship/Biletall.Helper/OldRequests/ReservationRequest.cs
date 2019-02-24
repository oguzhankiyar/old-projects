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
    public class ReservationRequest
    {
        public static bool IsReservationCompleted { get; private set; }
        public static Action<ActionResult> OnReservationCompleted = null;

        public static void GetReservation(Ticket ticket)
        {
            IsReservationCompleted = false;
            string xml;
            if (ticket.Type == TicketType.BusJourney)
                xml = ReservationParsing.GetBusReservation(ticket);
            else
                xml = ReservationParsing.GetAirplaneReservation(ticket);
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (c, e) =>
            {
                string xmlResult = e.Result;
                PopulateReservation(xmlResult);
            };
        }

        private static void PopulateReservation(string xmlResult)
        {
            var reservation = ReservationParsing.ParseReservation(xmlResult);
            IsReservationCompleted = true;
            if (OnReservationCompleted != null)
                OnReservationCompleted(reservation);
        }

        public static bool IsCancelReservationCompleted = false;
        public static Action<ActionResult> OnCancelReservationCompleted = null;

        public static void GetCancelReservation(Ticket ticket, Passenger passenger = null)
        {
            IsCancelReservationCompleted = false;
            string xml = ReservationParsing.GetCancelReservation(ticket, passenger);
            var client = new ServiceSoapClient();
            client.StrIsletAsync(xml, Global.Authorization);
            client.StrIsletCompleted += (c, e) =>
            {
                string xmlResult = e.Result;
                PopulateCancelReservation(xmlResult);
            };
        }

        private static void PopulateCancelReservation(string xmlResult)
        {
            var result = ReservationParsing.ParseCancelReservation(xmlResult);
            IsCancelReservationCompleted = true;
            if (OnCancelReservationCompleted != null)
                OnCancelReservationCompleted(result);
        }
    }
}
