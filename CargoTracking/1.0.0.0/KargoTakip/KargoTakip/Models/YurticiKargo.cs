using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KargoTakip.Models
{
    class YurticiKargo
    {
        public static Sirket Sirket = new Sirket() { Adi = "Yurtiçi Kargo", Resim = "/Assets/yurtici.png" };
        public static void TakipGetir(Dispatcher dispatcher, Grid grid, string kod)
        {
        }
    }
}
