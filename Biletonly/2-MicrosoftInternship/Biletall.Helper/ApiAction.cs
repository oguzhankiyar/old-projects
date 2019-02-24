using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biletall.Helper
{
    public static class ApiAction
    {
        public static int DeviceRegister = 1;
        public static int UserRegister = 2;
        public static int UserLogin = 3;
        public static int ControlVersion = 4;

        public static int GetCampaigns = 5;

        public static int GetBusFactories = 6;
        public static int GetBusFromStations = 7;
        public static int GetBusToStations = 8;
        public static int GetAirplaneStations = 9;

        public static int SearchBusTicket = 10;
        public static int SearchAirplaneTicket = 11;

        public static int GetBusDetails = 12;
        public static int GetAirplaneDetails = 13;

        public static int GetBusStops = 14;
        public static int GetServiceStops = 15;
        public static int GetBusSeatStates = 16;

        public static int PassengerVerification = 17;

        public static int BusReservation = 18;
        public static int AirplaneReservation = 19;

        public static int BusBuying = 20;
        public static int AirplaneBuying = 21;

        public static int BusSecureBuying = 22;
        public static int AirplaneSecureBuying = 23;

        public static int CancelReservation = 24;
        public static int CancelBuying = 25;

        public static int SearchTicket = 26;
        public static int SearchOpenTicket = 27;

        public static int GetCloudData = 28;
        public static int UpdateCloudData = 29;

        public static int SendLog = 30;
        public static int SendFeedback = 31;

        public static int GetAllActions = 32;
        public static int SendNotification = 33;
        public static int SendMail = 34;

        public static int GetVerificationCode = 35;
    }
}
