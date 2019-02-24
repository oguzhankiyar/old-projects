using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Entities
{
    public class Ticket
    {
        public PNR PNR { get; set; }
        public Bill Bill { get; set; }
        public List<Journey> Journeys { get; set; }
        public List<Passenger> Passengers { get; set; } 
        public TicketType Type { get; set; }
        public ActionType ActionType { get; set; }
        public CreditCard CreditCard { get; set; }

        public Ticket()
        {
            PNR = new PNR();
            Bill = new Bill();
            CreditCard = new CreditCard();
            Journeys = new List<Journey>();
            Passengers = new List<Passenger>();
        }
        
        //public DateTime MaxReservationDate { get; set; }

        public Price Price { get; set; }

        public bool Is3DBuyingActivated { get; set; }

        public bool Is3DBuyingRequired { get; set; }

        public bool IsBirthdayRequired { get; set; }

        public bool IsPassportRequired { get; set; }
    }
}
