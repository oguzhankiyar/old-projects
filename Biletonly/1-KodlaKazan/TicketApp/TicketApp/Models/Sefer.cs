using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketApp.Models
{
    public class Sefer
    {
        public int No { get; set; }
        public Firma Firma { get; set; }
        public string KalkisYeri { get; set; }
        public string VarisYeri { get; set; }
        public string IlkKalkisYeri { get; set; }
        public string SonVarisYeri { get; set; }
        public DateTime HareketSaati { get; set; }
        public DateTime Tarih { get; set; }
        public string Saat { get; set; }
        public DateTime SeyahatSuresi { get; set; }
        public int HatNo { get; set; }
        public int TakipNo { get; set; }
        public bool DoluMu { get; set; }
        public string Guzergah { get; set; }
        public string SeferTipi { get; set; }
        public Otobus Otobus { get; set; }
        public double Fiyat { get; set; }

        public int IslemTipi { get; set; }
    }
}
