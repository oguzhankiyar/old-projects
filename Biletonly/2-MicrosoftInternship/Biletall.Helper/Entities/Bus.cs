using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biletall.Helper.Entities
{
    public class Bus
    {
        public string Plate { get; set; }
        public BusType Type { get; set; }
        public string PhoneNumber { get; set; }
        public string PlatformNumber { get; set; }

        public string DriverName { get; set; }
        public List<BusProperty> Properties { get; set; }
        public List<Seat> Seats { get; set; }
        public bool Is3DBuyingActivated { get; set; }
        public bool Is3DSecureRequired { get; set; }

        public Bus()
        {
            this.Properties = new List<BusProperty>();
            this.Seats = new List<Seat>();
        }
    }
}
