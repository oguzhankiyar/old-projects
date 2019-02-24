using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketApp.Models
{
    public class Rezervasyon
    {
        public string PNR { get; set; }
        public bool Sonuc { get; set; }
        public string Mesaj { get; set; }
        public DateTime Opsiyon { get; set; }
        public string SeferTarih { get; set; }

    }
}
