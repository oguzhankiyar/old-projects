using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;

namespace TicketApp.Models
{
    public class SavedData
    {
        public PNR PNR { get; set; }

        public List<Station> Stations { get; set; }

        private List<Station> _airports;
        public List<Station> Airports
        {
            get { return _airports; }
            set
            {
                _airports = value;
                Biletall.Helper.Global.LoadStations(value);
            }
        }

        public TicketSearch BusSearch { get; set; }

        public TicketSearch AirplaneSearch { get; set; }

        public List<Passenger> FavoritePassengers { get; set; }

        public List<Ticket> Tickets { get; set; }

        public DateTime StationsUpdatedDate { get; set; }

        public bool IsSentLogs { get; set; }

        public bool IsInstallationCompleted { get; set; }

        public SavedData()
        {
            Logger.MethodCalled("SavedData()");
            Stations = new List<Station>();
            Airports = new List<Station>();
            FavoritePassengers = new List<Passenger>();
            Tickets = new List<Ticket>();
            IsSentLogs = true;
            StationsUpdatedDate = DateTime.Now.AddHours((-1 * Constraints.StationsMinimumUpdateHour) - 1);

            InitSavedSearch();
        }

        public void InitSavedSearch()
        {
            Logger.MethodCalled("SavedData.InitSavedSearch()");
            BusSearch = new TicketSearch()
            {
                From = new Station() { Name = "İSTANBUL", Type = StationType.Bus },
                To = new Station() { Name = "ANKARA", Type = StationType.Bus },
                DepartureDate = DateTime.Now,
                Type = TicketType.BusJourney
            };
            
            AirplaneSearch = new TicketSearch()
            {
                From = new Station() { Code = "IST", City = "İstanbul", Name = "Atatürk", Country = "Türkiye", IsInternational = false, Type = StationType.Airplane },
                To = new Station() { Code = "ESB", City = "Ankara", Name = "Esenboğa", Country = "Türkiye", IsInternational = false, Type = StationType.Airplane },
                DepartureDate = DateTime.Now,
                JourneyType = JourneyType.OneWay,
                Type = TicketType.AirplaneJourney,
                ReturnDate = DateTime.Now.AddDays(2),
                Passengers = new List<Passenger>() { new Passenger() { Type = PassengerType.Adult } }
            };
        }

    }
}
