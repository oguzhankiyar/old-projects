using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KargoTakip.Models
{
    public class Takip
    {
        public string Kod { get; set; }
        public Sirket Sirket { get; set; }
        public DateTime SonGiris { get; set; }

        //public string Gonderen { get; set; }
        //public string GonderenAdresi { get; set; }
        public string CikisTarihi { get; set; }
        public string CikisBirimi { get; set; }

        //public string Alici { get; set; }
        //public string AliciAdresi { get; set; }
        public string VarisBirimi { get; set; }

        //public string TeslimTipi { get; set; }
        //public string OdemeTuru { get; set; }
        //public string TeslimSuresi { get; set; }

        public string SonDurum { get; set; }
        public string Teslim { get; set; }

        public List<Hareket> Hareketler { get; set; }

        public Takip()
        {
            Hareketler = new List<Hareket>();
        }
    }
}