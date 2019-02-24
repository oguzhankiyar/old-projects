using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Biletall.Helper.Enums;
using Windows.UI;

namespace Biletall.Helper
{
    internal class Functions
    {
        public static TicketType GetTicketType(string type)
        {
            switch (type)
            {
                case "K":
                    return TicketType.BusJourney;
                case "T":
                    return TicketType.AirplaneJourney;
                case "H":
                    return TicketType.AirplaneJourney;
                case "S":
                    return TicketType.AirplaneJourney;
                case "G":
                    return TicketType.InternationalJourney;
                default:
                    return TicketType.BusJourney;
            }
        }

        public static JourneyTime GetTime(string value)
        {
            switch (value)
            {
                case "Sabah":
                    return JourneyTime.Morning;
                case "Öğle":
                    return JourneyTime.Afternoon;
                case "Gece":
                    return JourneyTime.Night;
                default:
                    return JourneyTime.Evening;
            }
        }

        public static SeatState GetSeatState(string state)
        {
            switch (state)
            {
                case "0":
                    return SeatState.Empty;
                case "1":
                    return SeatState.Male;
                case "2":
                    return SeatState.Female;
                default:
                    return SeatState.Resale;
            }
        }

        public static IPAddress GetIPAddress()
        {
            List<string> IpAddress = new List<string>();
            var Hosts = Windows.Networking.Connectivity.NetworkInformation.GetHostNames().ToList();
            foreach (var Host in Hosts.ToList())
            {
                string IP = Host.DisplayName;
                IpAddress.Add(IP);
            }
            IPAddress address = IPAddress.Parse(IpAddress.Last());
            return address;
        }

        public static PassengerType GetPassengerType(string type)
        {
            switch (type)
            {
                case "1":
                    return PassengerType.Adult;
                case "2":
                    return PassengerType.Child;
                case "3":
                    return PassengerType.Baby;
                case "4":
                    return PassengerType.Infant;
                case "5":
                    return PassengerType.Student;
                case "7":
                    return PassengerType.Military;
                case "8":
                    return PassengerType.Teenager;
                default:
                    return PassengerType.Adult;
            }
        }

        public static string GetPassengerType(PassengerType passengerType)
        {
            switch (passengerType)
            {
                case PassengerType.Adult:
                    return "1";
                case PassengerType.Child:
                    return "2";
                case PassengerType.Baby:
                    return "3";
                case PassengerType.Infant:
                    return "4";
                case PassengerType.Student:
                    return "5";
                case PassengerType.Military:
                    return "7";
                case PassengerType.Teenager:
                    return "8";
                default:
                    return null;
            }
        }

        public static ActionType GetActionType(string type)
        {
            switch (type)
            {
                case "REZERVASYON":
                    return ActionType.Reservation;
                case "REZIPTAL":
                    return ActionType.CanceledReservation;
                case "SATIS":
                    return ActionType.Buying;
                case "SATISIPTAL":
                    return ActionType.CanceledBuying;
                default:
                    return ActionType.Reservation;
            }
        }

        public static string TidyName(string text)
        {
            text = text.Replace("ç", "c");
            text = text.Replace("ı", "i");
            text = text.Replace("ğ", "g");
            text = text.Replace("ö", "o");
            text = text.Replace("ş", "s");

            text = text.Replace("Ç", "C");
            text = text.Replace("İ", "I");
            text = text.Replace("Ğ", "G");
            text = text.Replace("Ö", "O");
            text = text.Replace("Ş", "S");
            return text;
        }
    }
}
