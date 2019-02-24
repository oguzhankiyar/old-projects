using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Entities
{
    public class Segment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Factory Factory { get; set; }
        public Station From { get; set; }
        public Station To { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }

        public DateTime Duration { get; set; }

        // hiç kullanılmıyor, journeydetailspage'de gece kalkan seferler buradan kontrol edilebilir.
        public JourneyTime Time { get; set; }

        public int LineNumber { get; set; }

        public string Hour { get; set; }

        public Bus Bus { get; set; }

        public List<string> Route { get; set; }

        // hiç kullanılmıyor
        public int EmptySeatCount { get; set; }
        
        public SegmentType Type { get; set; }

        public List<JourneyClass> Classes { get; set; }

        public JourneyClass SelectedClass { get; set; }

        public string Code { get; set; }

        public Price UnitPrice { get; set; }

        public Segment()
        {
            Classes = new List<JourneyClass>();
        }

    }
}
