using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateDesignPattern
{
    abstract class SicakIcecek
    {
        protected virtual void SuKaynat()
        {
            Console.WriteLine("su kaynatiliyor..");
        }
        protected abstract void Hazirla();
        protected virtual void BardagaDoldur()
        {
            Console.WriteLine("bardaga dolduruluyor..");
        }
        protected virtual void ServisEt()
        {
            Console.WriteLine("icecek hazir! afiyet olsun :)");
        }
        public void SicakIcecekHazirla()
        {
            Console.WriteLine("\niceceginiz hazirlanmaya baslandi..\n");
            SuKaynat();
            Hazirla();
            BardagaDoldur();
            ServisEt();
        }
    }
}
