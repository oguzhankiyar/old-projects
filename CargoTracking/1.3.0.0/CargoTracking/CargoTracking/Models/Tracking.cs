using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTracking.Models
{
    public class Tracking
    {
        public string Code { get; set; }
        public Factory Factory { get; set; }
        public DateTime LastSearchDate { get; set; }
        public string OutputPlace { get; set; }
        public string OutputDate { get; set; }
        public string InputPlace { get; set; }
        public bool IsDelivered { get; set; }
        public string Receiver { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string LastState { get; set; }
        public List<Movement> Movements { get; set; }

        public Tracking()
        {
            this.Movements = new List<Movement>();
        }
    }
}
