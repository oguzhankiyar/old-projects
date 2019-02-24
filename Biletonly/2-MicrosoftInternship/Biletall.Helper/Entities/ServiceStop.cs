using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biletall.Helper.Entities
{
    public class ServiceStop
    {
        public string FullName
        {
            get
            {
                if (this.Hour != null)
                    return this.Location + " (" + ((DateTime)this.Hour).ToString("hh:mm") + ")";
                return this.Location;
            }
        }
        public string Location { get; set; }
        public DateTime? Hour { get; set; }
    }
}
