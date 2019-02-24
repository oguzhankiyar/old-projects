using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KargoTakip.Models
{
    class Data
    {
        public static List<Sirket> Sirketler
        {
            get
            {
                List<Sirket> _Sirketler = new List<Sirket>();
                _Sirketler.Add(ArasKargo.Sirket);
                _Sirketler.Add(DHLKargo.Sirket);
                _Sirketler.Add(MNGKargo.Sirket);
                _Sirketler.Add(PTTKargo.Sirket);
                _Sirketler.Add(SuratKargo.Sirket);
                //_Sirketler.Add(UPSKargo.Sirket);
                //_Sirketler.Add(YurticiKargo.Sirket);
                return _Sirketler;
            }
        }
        public static List<Takip> Gecmis = new List<Takip>();

        public static Sirket _Sirket = null;
        public static string _TakipKodu = null;
        public static Takip _TakipDetay = null;

    }
}
