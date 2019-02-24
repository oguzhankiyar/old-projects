using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OK.Transport.Web.ViewModels
{
    public class TransporterReservationViewModel
    {
        public int Id { get; set; }
        public PlaceViewModel From { get; set; }
        public PlaceViewModel To { get; set; }
        public List<StopViewModel> Stops { get; set; }

        public TransporterReservationViewModel()
        {
            this.Stops = new List<StopViewModel>();
        }
    }
}