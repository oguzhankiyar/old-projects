using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OK.Transport.Web.ViewModels
{
    public class TransporterViewModel
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string WorkingHours { get; set; }
        public int? BaggageCapacity { get; set; }
        public int PassengerCapacity { get; set; }
        public double Price { get; set; }
        public string SeatSchema { get; set; }
    }
}