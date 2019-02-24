using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Entities
{
    public class Passenger
    {
        public long? TCKN { get; set; }
        public string FullName { get { return (this.FirstName + " " + this.LastName); } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        public ServiceStop FromServiceStop { get; set; }
        public ServiceStop ToServiceStop { get; set; }
        public Seat Seat { get; set; }

        public Gender? Gender { get; set; }
        public string FactoryCardId { get; set; }
        public long PassaportId { get; set; }
        public DateTime? Birthday { get; set; }
        public PassengerType Type { get; set; }
        public Price Price { get; set; }
        public TicketAction LastAction { get; set; }
        public List<TicketAction> TicketActions { get; set; }

        public bool IsValidForBus
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || Birthday == null || TCKN == null || TCKN.ToString().Length < 11)
                    return false;
                return true;
            }
        }

        public bool IsValidForAirplane
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || Gender == null || Birthday == null || TCKN == null || TCKN.ToString().Length < 11)
                    return false;
                return true;
            }
        }

        public Passenger()
        {
            this.Gender = null;
            this.Birthday = null;
            this.TCKN = null;
        }


        public string ETicketId { get; set; }
    }
}