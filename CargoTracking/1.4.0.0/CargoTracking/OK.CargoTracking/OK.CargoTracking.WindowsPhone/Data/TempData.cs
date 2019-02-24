using OK.CargoTracking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OK.CargoTracking.WindowsPhone.Data
{
    public class TempData
    {
        public string Code { get; set; }
        public Factory Factory { get; set; }
        public Tracking Tracking { get; set; }
    }
}
