using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateDesignPattern
{
    class Nescafe : SicakIcecek
    {
        protected override void Hazirla()
        {
            Console.WriteLine("nescafe bardaga dokuluyor..");
        }
    }
}
