using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;

namespace TicketApp.Models
{
    public static class Extensions
    {
        public static string GetDescription(this ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.Buying:
                    return "Satış";
                case ActionType.Reservation:
                    return "Rezervasyon";
                case ActionType.CanceledReservation:
                    return "Rezervasyon iptal";
                case ActionType.CanceledBuying:
                    return "Satış iptal";
                default:
                    return null;
            }
        }

        public static string GetDescription(this TicketType ticketType)
        {
            switch (ticketType)
            {
                case TicketType.BusJourney:
                    return "Otobüs";
                case TicketType.AirplaneJourney:
                    return "Uçak";
                case TicketType.InternationalJourney:
                    return "Uçak (Yurt dışı)";
                default:
                    return null;
            }
        }

        public static string GetDescription(this PassengerType passengerType)
        {
            switch (passengerType)
            {
                case PassengerType.Adult:
                    return "Yetişkin";
                case PassengerType.Child:
                    return "Çocuk";
                case PassengerType.Baby:
                    return "Bebek";
                case PassengerType.Infant:
                    return "Yaşlı";
                case PassengerType.Student:
                    return "Öğrenci";
                case PassengerType.Military:
                    return "Asker";
                case PassengerType.Teenager:
                    return "Genç";
                default:
                    return null;
            }
        }

        public static string GetDescription(this DateTime dateTime)
        {
            if (dateTime == null)
                return null;

            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    return "perşembe'yi cuma'ya";
                case DayOfWeek.Monday:
                    return "pazar'ı pazartesi'ne";
                case DayOfWeek.Saturday:
                    return "cuma'yı cumartesi'ye";
                case DayOfWeek.Sunday:
                    return "cumartesi'yi pazar'a";
                case DayOfWeek.Thursday:
                    return "çarşamba'yı perşembe'ye";
                case DayOfWeek.Tuesday:
                    return "pazartesi'yi salı'ya";
                case DayOfWeek.Wednesday:
                    return "salı'yı çarşamba'ya";
                default:
                    return null;
            }
        }

        public static string GetLastState(this Ticket ticket)
        {
            var passengerList = ticket.Passengers.ToList();
            int totalCount = passengerList.Count();
            int reservationCount = passengerList.Count(x => x.LastAction.Type == ActionType.Reservation);
            int canceledReservationCount = passengerList.Count(x => x.LastAction.Type == ActionType.CanceledReservation);
            int buyingCount = passengerList.Count(x => x.LastAction.Type == ActionType.Buying);
            int canceledBuyingCount = passengerList.Count(x => x.LastAction.Type == ActionType.CanceledBuying);

            if (buyingCount == totalCount)
                return "Satış";
            else if (reservationCount == totalCount)
                return "Rezervasyon";
            else if (canceledBuyingCount == totalCount)
                return "İptal edilmiş satış";
            else if (canceledReservationCount == totalCount)
                return "İptal edilmiş rezervasyon";
            else if (buyingCount != 0)
                return "Satış (Kısmi)";
            else if (reservationCount != 0)
                return "Rezervasyon (Kısmi)";
            else if (canceledReservationCount != 0)
                return "İptal edilmiş rezervasyon (Kısmi)";
            else if (canceledBuyingCount != 0)
                return "İptal edilmiş satış (Kısmi)";
            else
                return "Belirlenemedi";
        }
    }
}
