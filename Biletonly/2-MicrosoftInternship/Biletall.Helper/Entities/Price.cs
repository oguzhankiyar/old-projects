using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biletall.Helper.Entities
{
    public class Price
    {
        public int PassengerCount { get; set; }
        public double TotalPrice { get; set; }
        public double NetPrice { get; set; }
        public double Tax { get; set; }
        public double ServicePrice { get; set; }
    }
}
