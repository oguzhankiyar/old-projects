
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;

namespace TicketApp.Models
{
    public class TempData
    {
        public TicketSearch TicketSearch { get; set; }
        public List<Journey> Journeys { get; set; }


        public Ticket Ticket { get; set; }

        public Gender? SelectedGender { get; set; }

        public List<ServiceStop> FromServices { get; set; }

        public List<ServiceStop> ToServices { get; set; }

        public PNR PNR { get; set; }

        public TempData()
        {
            Logger.MethodCalled("TempData()");
            TicketSearch = new TicketSearch() { Passengers = new List<Passenger>() { new Passenger() { Type = PassengerType.Adult } } };
        }
    }
}
