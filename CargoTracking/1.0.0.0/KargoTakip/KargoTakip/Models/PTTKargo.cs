using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KargoTakip.Models
{
    class PTTKargo
    {
        public static Sirket Sirket = new Sirket() { Adi = "PTT Kargo", Resim = "/Assets/ptt.png" };

        public static void TakipGetir(Dispatcher dispatcher, Grid grid, string kod)
        {
        }
    }
}
