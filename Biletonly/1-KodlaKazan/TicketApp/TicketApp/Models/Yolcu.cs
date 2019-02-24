using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketApp.Models
{
    public class Yolcu
    {
        public Koltuk Koltuk { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string BinisServisi { get; set; }
    }
}
