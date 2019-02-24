using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Entities
{
    public class Station
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string FullName {
            get
            {
                if (this.Type == StationType.Bus)
                    return (this.Name);
                else
                    return (this.City + ", " + this.Name + " (" + this.Code + ")");
            }
        }
        public bool IsInternational { get; set; }
        public StationType Type { get; set; }

        public Station()
        {

        }

        public Station(string name)
        {
            this.Name = name;
        }
    }
}
