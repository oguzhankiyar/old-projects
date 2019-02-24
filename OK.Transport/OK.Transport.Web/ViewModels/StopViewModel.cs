using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OK.Transport.Web.ViewModels
{
    public class StopViewModel
    {
        public int Sort { get; set; }
        public PlaceViewModel Place { get; set; }
        public int PassengerCountGetOn { get; set; }
        public int PassengerCountGetOff { get; set; }
    }
}