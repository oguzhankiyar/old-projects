using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateDesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ne icmek istesiniz?");
            Console.WriteLine("1. Cay");
            Console.WriteLine("2. Nescafe");
            int secim = Convert.ToInt32(Console.ReadLine());
            if (secim == 1)
            {
                Cay cay = new Cay();
                cay.SicakIcecekHazirla();
            }
            else if (secim == 2)
            {
                Nescafe kahve = new Nescafe();
                kahve.SicakIcecekHazirla();
            }
            Console.ReadKey();
        }
    }
}
