using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateDesignPattern
{
    class Cay : SicakIcecek
    {
        protected override void Hazirla()
        {
            Console.WriteLine("demlige cay konuluyor..");
        }
    }
}
