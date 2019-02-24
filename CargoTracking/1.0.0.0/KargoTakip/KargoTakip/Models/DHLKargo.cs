using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KargoTakip.Models
{
    class DHLKargo
    {
        public static Sirket Sirket = new Sirket() { Adi = "DHL Kargo", Resim = "/Assets/dhl.png" };

        public static void TakipGetir(Dispatcher dispatcher, Grid grid, string kod)
        {
        }
    }
}
