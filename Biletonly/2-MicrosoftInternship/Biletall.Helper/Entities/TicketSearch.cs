using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Entities
{
    public class TicketSearch
    {
        public Factory Factory { get; set; }
        public Station From { get; set; }
        public Station To { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<Passenger> Passengers { get; set; }
        public JourneyType JourneyType { get; set; }
        public TicketType Type { get; set; }

        public ActionType ActionType { get; set; }

        public TicketSearch()
        {
            this.Passengers = new List<Passenger>();
        }
    }
}
