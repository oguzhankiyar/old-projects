using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OK.Transport.Web.ViewModels
{
    public class SearchResultViewModel
    {
        public PlaceViewModel From { get; set; }
        public PlaceViewModel To { get; set; }
        public double Distance { get; set; }
        public List<TransporterViewModel> Transporters { get; set; }

        public SearchResultViewModel()
        {
            this.Transporters = new List<TransporterViewModel>();
        }
    }
}