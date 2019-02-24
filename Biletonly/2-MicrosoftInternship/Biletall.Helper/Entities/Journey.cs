using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Entities
{
    public class Journey
    {
        public string Id { get; set; }
        public Factory Factory { get; set; }
        public Station From { get; set; }
        public Station To { get; set; }
        public DateTime DepartureDate { get; set; }
        public List<Segment> Segments { get; set; }

        // ticket içine taşınacak
        public DateTime MaxReservationDate { get; set; }

        public Price Price { get; set; }

        public bool IsFull { get; set; }
        public bool IsReturn { get; set; }


        public Journey()
        {
            Segments = new List<Segment>();
        }

        public Journey Clone()
        {
            var clone = (Journey)this.MemberwiseClone();
            return clone;
        }

    }
}
