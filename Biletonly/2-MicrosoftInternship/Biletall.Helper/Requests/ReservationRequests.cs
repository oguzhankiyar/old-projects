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
    public static class ReservationRequests
    {
        public static BaseRequest<ActionResult> ReservationRequest;
        public static BaseRequest<ActionResult> CancelReservationRequest;

        static ReservationRequests()
        {
            ReservationRequest = BaseRequest<ActionResult>.GetInstance();
            ReservationRequest.IsCancelable = false;
            CancelReservationRequest = BaseRequest<ActionResult>.GetInstance();
            CancelReservationRequest.IsCancelable = false;
        }

        public static void GetReservation(Ticket ticket)
        {
            PassengerRequests.OnListVerificationCompleted = (stateResponse) =>
            {
                var state = stateResponse.Result;
                if (!state && ReservationRequest.OnCompleted != null)
                {
                    var response = new BaseResponse<ActionResult>();
                    response.Status = ResponseStatus.InvalidTCKN;
                    ReservationRequest.OnCompleted(response);
                }
                else
                {
                    string xml;
                    if (ticket.Type == TicketType.BusJourney)
                        xml = ReservationParsings.GetBusReservation(ticket);
                    else
                        xml = ReservationParsings.GetAirplaneReservation(ticket);

                    ReservationRequest.OnPopulated = (xmlResult) =>
                    {
                        ReservationRequest.Response = ReservationParsings.ParseReservation(xmlResult);
                    };
                    Global.OnRequestCalled("ReservationRequests.GetReservation", new object[] { ticket });
                    if (ticket.Type == TicketType.BusJourney)
                        ReservationRequest.GetResult(xml, ApiAction.BusReservation);
                    else
                        ReservationRequest.GetResult(xml, ApiAction.AirplaneReservation);
                }
            };
            PassengerRequests.VerifyPassengers(ticket.Passengers);
        }

        public static void GetCancelReservation(Ticket ticket, Passenger passenger = null)
        {
            string xml = ReservationParsings.GetCancelReservation(ticket, passenger);
            CancelReservationRequest.OnPopulated = (xmlResult) =>
            {
                CancelReservationRequest.Response = ReservationParsings.ParseCancelReservation(xmlResult);
            };
            Global.OnRequestCalled("ReservationRequests.GetCancelReservation", new object[] { ticket, passenger });
            CancelReservationRequest.GetResult(xml, ApiAction.CancelReservation);
        }
    }
}
